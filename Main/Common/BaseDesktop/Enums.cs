﻿namespace UtnEmall.Server.EntityModel
{
    public enum ComponentType
    {
        None = 0,
        ListForm = 1,
        MenuForm = 2,
        EnterSingleDataFrom = 3,
        ShowDataForm = 4,
        DataSource = 5,
        FormMenuItem = 6,
        TemplateListItem = 7,
        Table = 8,
    }

    public enum CivilState
    {
        Single = 0,
        Married = 1,
        Widow = 2,
        Divorced = 3,
    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
    }
}

namespace UtnEmall.Server.Base
{
    public enum OperatorType
    {
        Equal, NotEqual, Less, Greater, LessOrEqual, GreaterOrEqual, Like, NotLike
    }
}