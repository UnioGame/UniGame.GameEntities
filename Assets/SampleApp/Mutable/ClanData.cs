using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RemoteDataModule.RemoteDataTypes;

public class ClanData {
    // TO DO добавить поддержку словарей
    public string ClanName;
    public int ClanRating;
    public List<ClanUserReference> Users;
}
