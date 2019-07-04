const functions = require('firebase-functions');

const admin = require('firebase-admin');
admin.initializeApp();

const USERS_PER_ROOM = 5;
const PVP_POOL_PATH = "PvpPool/";
const ROOMS_PATH = PVP_POOL_PATH + "Rooms/";
const ROOMS_META_PATH = PVP_POOL_PATH + "RoomsMeta/" // свободные комнаты, общее число
const ROOMS_LINKS_PATH = PVP_POOL_PATH + "RoomLinks/";
const ROOMS_COUNTS_PATH = PVP_POOL_PATH + "RoomsCount/";

exports.registerUserForPvp = functions.https.onCall((data, context) => {
    let userId = data.UserId;
    console.log(`Register user for pvp ${userId}`);
    return admin.database().ref(ROOMS_LINKS_PATH + userId).once("value")
    .then((refData)=>{
        if(refData.exists()){
          console.log(`User :: ${userId} already registered`);
          return;
        }
        return admin.database().ref(ROOMS_META_PATH)
          .once('value')
          .then((roomsMeta)=>{
            let roomsCount = 0
            if(roomsMeta.child("Count").exists())
              roomsCount = parseInt(roomsMeta.child("Count").val());
            let roomId = -1;
            if(roomsMeta.child("NotFilledRooms").exists()){
              var candidates = roomsMeta.child("NotFilledRooms");
              candidates.forEach((child)=>{
                if(roomId < 0 || parseInt(child.key) < roomId )
                  roomId = parseInt(child.key);
              });
            }
            if(roomId < 0){
              roomId = roomsCount;
              // TO DO Transaction
              admin.database().ref(ROOMS_META_PATH + "Count").set(roomsCount+1);
            }
            // Add user link
            admin.database().ref(ROOMS_LINKS_PATH + userId).set(roomId);
            // Add user to room
            return admin.database().ref(ROOMS_PATH + roomId + "/" + userId).set(true);
            });
        
    });
  });

exports.onUserAddedToRoom = functions.database.ref(`${ROOMS_PATH}{RoomId}/{UserId}`).onCreate((_,context)=>{
  let roomId = context.params.RoomId;
  let roomCount = 0;  
  return admin.database().ref(ROOMS_COUNTS_PATH + roomId).transaction((count)=>{
      let c = count !== null? parseInt(count):0;
      roomCount = c + 1;
      return c + 1;

  }).then(()=>{
      if(roomCount >= USERS_PER_ROOM)
        return admin.database().ref(ROOMS_META_PATH + "NotFilledRooms" + "/" + roomId ).remove();
      else
      if(roomCount <= 0)
        admin.database().ref(ROOMS_META_PATH + "EmptyRooms/"+roomId).set(true)
      else
        admin.database().ref(ROOMS_META_PATH + "EmptyRooms/"+roomId).remove();
      return admin.database().ref(ROOMS_META_PATH + "NotFilledRooms" + "/" + roomId ).set(true);
  })
});

exports.onUserRemovedFromRoom = functions.database.ref(`${ROOMS_PATH}{RoomId}/{UserId}`).onDelete((_,context)=>{
  let roomId = context.params.RoomId;
  let roomCount = 0;  
  return admin.database().ref(ROOMS_COUNTS_PATH + roomId).transaction((count)=>{
      let c = count !== null? parseInt(count):0;
      roomCount = c - 1;
      return c - 1;

  }).then(()=>{
      if(roomCount >= USERS_PER_ROOM)
        return admin.database().ref(ROOMS_META_PATH + "NotFilledRooms" + "/" + roomId ).remove();
      else{
        if(roomCount <= 0)
            admin.database().ref(ROOMS_META_PATH + "EmptyRooms/"+roomId).set(true)
        else
            admin.database().ref(ROOMS_META_PATH + "EmptyRooms/"+roomId).remove();
        return admin.database().ref(ROOMS_META_PATH + "NotFilledRooms" + "/" + roomId ).set(true);
      }
  })
});

exports.unregisterUserForPvp = functions.https.onCall((data, context) => {
  let userId = data.UserId;
  console.log(`UnRegister user for pvp ${userId}`);
  return admin.database().ref(ROOMS_LINKS_PATH + userId).once("value")
  .then((refData)=>{
      if(!refData.exists()){
        console.log(`User :: ${userId} not registered`);
        return;
      }
      admin.database().ref(ROOMS_LINKS_PATH + userId).remove();
      return admin.database().ref(ROOMS_PATH + refData.val() + "/" + userId).remove();
    });
  });