using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
   void loadData(GameData data);

   void saveData( GameData data);
}
