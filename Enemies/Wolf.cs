using JYW.RandomSurvival.Commmons;

public class Wolf : EnemyBase
{
    protected override float hp { get; set; } = 15;
    public override float power { get; set; } =1;
    public override int EXP { get; set; } = 10;
    public override EnemyType enemyType { get; set; } = EnemyType.Chase;

}
