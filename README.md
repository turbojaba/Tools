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
