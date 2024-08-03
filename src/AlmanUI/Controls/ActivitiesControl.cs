using Business;
using Alman.SharedModels;
using System.Collections.Generic;
using Alman.SharedDefinitions;
using System.Linq;
using System.Diagnostics;
namespace AlmanUI.Controls;

public static class ActvitiesControl
{
    public static IReadOnlyList<IActivityBase> GetActivities()
    {
        return BusinessActivitiesApi.GetActivities();
    }

    private static ReturnCode AddActivities(IReadOnlyList<IActivityBase> activities)
    {
        return BusinessActivitiesApi.AddActivities(activities);
    }

    public static ReturnCode SaveActivities(IReadOnlyList<IActivityBase> activitiesToSave, IList<int> activitiesIdsToDelete)
    {
        if (activitiesIdsToDelete.Any())
        {
            ReturnCode retCode = DeleteActivities(activitiesIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IActivityBase)}'s.");
                return retCode;
            }
        }
        var activitiesFromDb = BusinessActivitiesApi.GetActivities();
        var activitiesIdsFRomDb = (from dbAct in activitiesFromDb select dbAct.ActivityId).ToList();
        
        var updatedActivities = (from act in activitiesToSave where activitiesIdsFRomDb.Contains(act.ActivityId) select act).ToList();
        
/*        foreach (var activity in activitiesFromDb)
        {
            var updatedActivity = activities.Single(act => act.ActivityId == activity.ActivityId);
            activity.ActivityPrice = updatedActivity.ActivityPrice;
            activity.ActivityName = updatedActivity.ActivityName;
        }*/
        BusinessActivitiesApi.UpdateActivities(updatedActivities);

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

    private static ReturnCode DeleteActivities(IList<int> activitiesIds)
    {
        return BusinessActivitiesApi.DeleteActivities(activitiesIds);
    }

}

