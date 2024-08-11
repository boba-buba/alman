using Business;
using Alman.SharedModels;
using DatabaseAccess;
using DbAccess.Models;

using System.Collections.Generic;
using Alman.SharedDefinitions;
using System.Linq;
using System.Diagnostics;
namespace AlmanUI.Controls;



public class ActivitiesControl : ControlBase<DbAccess.Models.Activity, IActivityBase>
{
    
    public static ReturnCode SaveActivities(IReadOnlyList<IActivityBase> activitiesToSave, IList<int> activitiesIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (activitiesIdsToDelete.Any())
        {
            retCode = DeleteItems(activitiesIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IActivityBase)}'s.");
                return retCode;
            }
        }
        var activitiesFromDb = GetItems();
        var activitiesIdsFRomDb = (from dbAct in activitiesFromDb select dbAct.Id).ToList();
        
        var updatedActivities = (from act in activitiesToSave where activitiesIdsFRomDb.Contains(act.Id) select act).ToList();
        
        retCode = UpdateItems(updatedActivities);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong wile updating {nameof(IActivityBase)}'s.");
            return retCode;
        }
        
        
        if (activitiesFromDb.Count < activitiesToSave.Count)
        { 
            var newActivities = new List<IActivityBase>();
            for (int i = activitiesFromDb.Count; i < activitiesToSave.Count; i++)
            {
                newActivities.Add(activitiesToSave[i]);
            }
            retCode = AddItems(newActivities);
        }

        return retCode;
    }

}

