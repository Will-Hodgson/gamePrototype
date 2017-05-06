namespace Assets.Scripts
{
    public interface IPlayable {
        string name { get; set; }
        int manaCost { get; set; }
        string Type();
        void OnDraw();
        void OnCast();
        void OnEnterGraveyard();
    }
}