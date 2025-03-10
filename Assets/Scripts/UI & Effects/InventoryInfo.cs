using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;


public class InventoryInfo : MonoBehaviour

{   
    private SirGluten sirGluten;
    [SerializeField] private TextMeshProUGUI mainName,mainDesc,subName,subDesc, passiveName, passiveDesc;

    void Start(){
        sirGluten = GameObject.Find("SirGluten").GetComponent<SirGluten>();
    }

    void Update() {
        if (sirGluten.MainSlot != null) {
            Weapon mainWeapon = sirGluten.MainSlot.GetComponent<Weapon>();
            mainName.text = mainWeapon.WeaponName;
            string description = $"{mainWeapon.Description}";
            if (mainWeapon != null) {
                description += $"\nAtk: {mainWeapon.AttackDamage} - Flavor: {mainWeapon.Flavor}";
            }
            Magic magic = mainWeapon.GetComponent<Magic>();
            if (magic != null) description += $" - Glucose: {magic.GlucoseCost}";
            mainDesc.text = description;
        } else {
            mainName.text = "<empty>";
            mainDesc.text = "";
        }

        if (sirGluten.SubSlot != null) {
            Weapon subWeapon = sirGluten.SubSlot.GetComponent<Weapon>();
            subName.text = subWeapon.WeaponName;
            string description = "";
            if (subWeapon != null) {
                description += $"\nAtk: {subWeapon.AttackDamage} - Flavor: {subWeapon.Flavor}";
            }
            Magic magic = subWeapon.GetComponent<Magic>();
            if (magic != null) description += $" - Glucose: {magic.GlucoseCost}";
            subDesc.text = description;
        } else {
            subName.text = "<empty>";
            subDesc.text = "";
        }
            
        if (sirGluten.PassiveSlot != null) {
            passiveName.text = sirGluten.PassiveSlot.PassiveName;
            passiveDesc.text = sirGluten.PassiveSlot.Description;
        } else {
            passiveName.text = "<empty>";
            passiveDesc.text = "";
        }
    }

    
}
