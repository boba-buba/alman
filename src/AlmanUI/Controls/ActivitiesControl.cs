using Business;
using Alman.SharedModels;
using System.Collections.Generic;
using Alman.SharedDefinitions;
using System.Linq;
using System.Diagnostics;
namespace AlmanUI.Controls;

public static class ActivitiesControl
{
    public static IReadOnlyList<IActivityBase> GetActivities() =>
        BusinessActivitiesApi.GetActivities();
    

    private static ReturnCode AddActivities(IReadOnlyList<IActivityBase> activities) =>
        BusinessActivitiesApi.AddActivities(activities);

    private static ReturnCode DeleteActivities(IList<int> activitiesIds) =>
        BusinessActivitiesApi.DeleteActivities(activitiesIds);


    public static ReturnCode SaveActivities(IReadOnlyList<IActivityBase> activitiesToSave, IList<int> activitiesIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (activitiesIdsToDelete.Any())
        {
            retCode = DeleteActivities(activitiesIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IActivityBase)}'s.");
                return retCode;
            }
        }
        var activitiesFromDb = BusinessActivitiesApi.GetActivities();
        var activitiesIdsFRomDb = (from dbAct in activitiesFromDb select dbAct.ActivityId).ToList();
        
        var updatedActivities = (from act in activitiesToSave where activitiesIdsFRomDb.Contains(act.ActivityId) select act).ToList();
        
        retCode = BusinessActivitiesApi.UpdateActivities(updatedActivities);
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
            AddActivities(newActivities);
        }

        return ReturnCode.OK;
    }

}

