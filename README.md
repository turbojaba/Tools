# Tools

Адекватные линии в DataGrid'е
```xaml
<Application.Resources>
    <Style TargetType="DataGrid">
        <Setter Property="VerticalGridLinesBrush">
            <Setter.Value>
                <SolidColorBrush Color="{DynamicResource {x:Static SystemColors.AppWorkspaceColorKey}}"/>
            </Setter.Value>
        </Setter>
        <Setter Property="HorizontalGridLinesBrush">
            <Setter.Value>
                <SolidColorBrush Color="{DynamicResource {x:Static SystemColors.ActiveBorderColorKey}}"/>
            </Setter.Value>
        </Setter>
    </Style>
</Application.Resources>
```

```csharp
private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
```

```
magick convert -background none IMAGE.svg -define icon:auto-resize IMAGE.ico
```

``` csharp
//avoid a "object reference not set to an instance of an object@ exception in XAML code while design time
if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
```
