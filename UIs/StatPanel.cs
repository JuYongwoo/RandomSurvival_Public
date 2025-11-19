using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.RandomSurvival.UIs
{
    public class StatPanel : MonoBehaviour
    {

        private enum StatPanelEnum
        {
            LvText,
            ExpText,
            WeaponImg,
            WeaponNameTxt,
            WeaponDmgTxt
        }

        private Dictionary<StatPanelEnum, GameObject> statPanelMap;

        private void Awake()
        {
            statPanelMap = Util.MapEnumChildObjects<StatPanelEnum, GameObject>(this.gameObject);

            EventManager.Instance.UpdateEXPUIEvent -= ChangeEXP;
            EventManager.Instance.UpdateEXPUIEvent += ChangeEXP;
            EventManager.Instance.UpdateWeaponUIEvent -= ChangeWeapon;
            EventManager.Instance.UpdateWeaponUIEvent += ChangeWeapon;
        }

        private void OnDestroy()
        {
            EventManager.Instance.UpdateEXPUIEvent -= ChangeEXP;
            EventManager.Instance.UpdateWeaponUIEvent -= ChangeWeapon;
        }

        private void ChangeEXP(float currentEXP, float maxEXP)
        {
            if (statPanelMap[StatPanelEnum.LvText] == null
                || statPanelMap[StatPanelEnum.ExpText] == null) return;

            int level = (int)(currentEXP / maxEXP) + 1;
            float expInCurrentLevel = currentEXP % maxEXP;

            statPanelMap[StatPanelEnum.LvText].GetComponent<Text>().text = $"Lv.{level}";
            statPanelMap[StatPanelEnum.ExpText].GetComponent<Text>().text = $"Exp {expInCurrentLevel}/{maxEXP}";
        }


        private void ChangeWeapon(Weapons weaponName)
        {
            if (statPanelMap[StatPanelEnum.WeaponImg] == null
                || statPanelMap[StatPanelEnum.WeaponNameTxt] == null
                || statPanelMap[StatPanelEnum.WeaponDmgTxt] == null) return;
            statPanelMap[StatPanelEnum.WeaponImg].GetComponent<Image>().sprite = ResourceManager.Instance.GetWeaponData(weaponName).GetWeaponIcon();
            statPanelMap[StatPanelEnum.WeaponNameTxt].GetComponent<Text>().text = ResourceManager.Instance.GetWeaponData(weaponName).GetWeaponName();
            statPanelMap[StatPanelEnum.WeaponDmgTxt].GetComponent<Text>().text = $"Damage {ResourceManager.Instance.GetWeaponData(weaponName).GetBaseDamage().ToString()}";

        }
    }
}