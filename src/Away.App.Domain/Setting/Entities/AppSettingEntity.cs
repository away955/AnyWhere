namespace Away.App.Domain.Setting.Entities;

[SugarTable("app_settings")]
public sealed class AppSettingEntity
{
    [SugarColumn(IsPrimaryKey = true)]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
