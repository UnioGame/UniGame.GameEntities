const functions = require('firebase-functions');
const admin = require('firebase-admin');
admin.initializeApp(functions.config().firebase);

const PVP_DATA_PATH = "PvpData/";
const PVP_ROOMS_PATH = "PvpData/Rooms/";
const PVP_ROOMS_COUNT_PATH = "PvpData/RoomsCount/";
const PVP_USER_REFS_PATH = "PvpData/UserReferences/";
const MAX_USER_PER_ROOM = 3;

exports.registerUserForPvp = functions.https.onCall((data) => {
    let userId = data.UserId;
    // ensure user not participate
    let idSnap = await admin.database().ref(PVP_USER_REFS_PATH + userId).once('value');
    if(idSnap.exists()){
        let roomId = idSnap.val();
        console.log(`User : ${userId} : already participate room :: ${roomId}`);
        return { RoomId : roomId};
    }

    // get minimal room with free slots

    // push user

    // add ref


    return {
        RoomId: 0
    };
});

exports.unregisterUserForPvp = functions.https.onCall((data) => {
    let userId = data.UserId;
    let idSnap = await admin.database().ref(PVP_USER_REFS_PATH + userId).once('value');
    if(!idSnap.exists())
        console.log(`No PVP reference stored for user : ${userId}`);
    let roomId = idSnap.val();
    await admin.database().ref(PVP_ROOMS_PATH + roomId + `/${userId}`).remove();
    await admin.database().ref(PVP_USER_REFS_PATH + userId).remove();
    // decrement roomCount
    // push to free list
    return {};
});
