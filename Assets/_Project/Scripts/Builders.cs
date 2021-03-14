namespace _Project.Test
{
    public static class A
    {
        public static AchievementBuilder Achievement()
        {
            return new AchievementBuilder();
        }
    }
    
    public static class An
    {
        public static AchievementConditionsBuilder AchievementCondition()
        {
            return new AchievementConditionsBuilder();
        }
        
        public static AchievementBuilder Achievement()
        {
            return new AchievementBuilder();
        }
    }
}