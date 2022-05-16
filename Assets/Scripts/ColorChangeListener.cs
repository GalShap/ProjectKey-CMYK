using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ColorChangeListener
{
    public void OnColorChange(ColorManager.ColorLayer layer);
}
