﻿using Chroomsoft.Top2000.Data.ClientDatabase.Sources;

namespace Chroomsoft.Top2000.Apps;

public partial class App : Application
{
    public static readonly IFormatProvider DateTimeFormatProvider = DateTimeFormatInfo.InvariantInfo;

    public static readonly IFormatProvider NumberFormatProvider = NumberFormatInfo.InvariantInfo;

    public App(IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyDataSource)
    {
        InitializeComponent();

        MainPage = new AppShell(updateClientDatabase, top2000AssemblyDataSource);
    }
}