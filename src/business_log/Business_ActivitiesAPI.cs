using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedModels;
using Alman.SharedDefinitions;
using System.Collections.ObjectModel;
namespace Business;


public static class BusinessActivitiesAPI
{
    public static IReadOnlyList<IActivityBase> GetActivities()
    {
        var db = new DbChildren();
        return db.GetActivities(act => true);
    }

    public static ReturnCode DeleteActivities(IList<int> activitiesIds)
    {
        var db = new DbChildren();
        var activitiesToDelete = db.GetActivities(act => activitiesIds.Contains(act.ActivityId));
        return db.DeleteActvities(activitiesToDelete);
    }

    public static ReturnCode AddActivities(IReadOnlyCollection<IActivityBase> newActivities)
    {
        var db = new DbChildren();
        Collection<Activity> activities = new Collection<Activity>();
        foreach (var activity in newActivities)
        {
            activities.Add(new Activity { ActivityName = activity.ActivityName, ActivityPrice = activity.ActivityPrice });
        }
        return db.AddActvities(activities);
    }

    public static ReturnCode UpdateActivities(IReadOnlyList<IActivityBase> updatedActivities)
    {
        var db = new DbChildren();
        var activitiesToUpdate = db.GetActivities(act => true);

        foreach (var activity in activitiesToUpdate)
        {
            var updatedActivity = updatedActivities.Single(act => act.ActivityId == activity.ActivityId);
            activity.ActivityPrice = updatedActivity.ActivityPrice;
            activity.ActivityName = updatedActivity.ActivityName;
        }
        return db.UpdateActvities(activitiesToUpdate);
    }
}