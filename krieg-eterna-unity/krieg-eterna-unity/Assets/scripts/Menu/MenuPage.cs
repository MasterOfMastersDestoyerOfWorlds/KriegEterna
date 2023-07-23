using System.Collections.Generic;
public class MenuPage{
    public List<MenuButton> buttons;
    public bool active;
    public MenuPage(bool active, List<MenuButton> buttons){
        this.buttons = buttons;
        this.active = active;
    }
}