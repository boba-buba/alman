using Business;
using Alman.SharedModels;
using System.Collections.Generic;
using Alman.SharedDefinitions;
using System.Linq;
namespace AlmanUI.Controls;

public static class ActvitiesControl
{
    public static IReadOnlyList<IActivityBase> GetActivities()
    {
        return BusinessActivitiesAPI.GetActivities();
    }

    private static ReturnCode AddActivities(IReadOnlyList<IActivityBase> activities)
    {
        return BusinessActivitiesAPI.AddActivities(activities);
    }

    public static ReturnCode SaveActivities(IReadOnlyList<IActivityBase> activities, IList<int> activitiesIdsToDelete)
    {
        if (activitiesIdsToDelete.Any())
        {
            DeleteActivities(activitiesIdsToDelete);
        }

        var activitiesFromDb = BusinessActivitiesAPI.GetActivities();
        foreach (var activity in activitiesFromDb)
        {
            var updatedActivity = activities.Single(act => act.ActivityId == activity.ActivityId);
            activity.ActivityPrice = updatedActivity.ActivityPrice;
            activity.ActivityName = updatedActivity.ActivityName;
        }
        BusinessActivitiesAPI.UpdateActivities(activitiesFromDb);

        if (activitiesFromDb.Count < activities.Count)
        { 
            var newActivities = new List<IActivityBase>();
            for (int i = activitiesFromDb.Count; i < activities.Count; i++)
            {
                newActivities.Add(activities[i]);
            }
            AddActivities(newActivities);
        }

        return ReturnCode.OK;
    }

    private static ReturnCode DeleteActivities(IList<int> activitiesIds)
    {
        return BusinessActivitiesAPI.DeleteActivities(activitiesIds);
    }

}

