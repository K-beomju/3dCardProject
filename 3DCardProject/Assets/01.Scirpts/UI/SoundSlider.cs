using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider fxSlider;

    public Text masterText;
    public Text bgmText;
    public Text fxText;

    private List<Slider> sliders;

    private void Start()
    {
   

        sliders = new List<Slider>
        {
            masterSlider,
            bgmSlider,
            fxSlider
        };

        sliders.ForEach((x) =>
        x.onValueChanged.AddListener((value) =>
        {
            x.value = value;
            AdjustVolumes();
        }));

        masterSlider.value = SoundManager.Instance.MasterVoulme;
        bgmSlider.value = SoundManager.Instance.BGMVolume;
        fxSlider.value = SoundManager.Instance.FxVoulme;
    }

    private void AdjustVolumes()
    {
        SoundManager.Instance.AdjustMasterVolume(masterSlider.value, masterText, bgmText, fxText);
        SoundManager.Instance.AdjustBGMVolume(bgmSlider.value);
        SoundManager.Instance.AdjustFxVoulme(fxSlider.value);
    }
}
