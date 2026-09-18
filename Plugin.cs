using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using UnityEngine;

namespace SeriKatilMod
{
    [BepInPlugin("com.kral.serikatil", "Seri Katil Modu", "1.0.0")]
    [BepInProcess("Among Us.exe")]
    public class Plugin : BasePlugin
    {
        public Harmony Harmony { get; } = new Harmony("com.kral.serikatil");

        public override void Load()
        {
            CustomRoleManager.RegisterRole<SeriKatilRole>();
            Harmony.PatchAll();
            Log.LogInfo("SERİ KATİL YÜKLENDİ! ELEKTRİK ODASINI MEZARLIĞA ÇEVİRME VAKTİ!");
        }
    }

    // Adam kestiğimiz anı yakalayan kanca:
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPatch
    {
        public static void Postfix(PlayerControl __instance, PlayerControl target)
        {
            // Eğer kesen kişi "Seri Katil" ise:
            var seriKatil = __instance.GetCustomRole<SeriKatilRole>();
            if (seriKatil != null)
            {
                seriKatil.BiriniKestim(__instance);
            }
        }
    }

    // Oyun esnasında kill süresi geri saymaya kalkarsa onu hep SIFIRDA tutan kanca:
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class KillTimerPatch
    {
        public static void Prefix(PlayerControl __instance, ref float time)
        {
            var seriKatil = __instance.GetCustomRole<SeriKatilRole>();
            if (seriKatil != null && seriKatil.KalanMermi > 0)
            {
                // Oyun cooldown koymaya çalışsa bile zorla 0 yapıyoruz!
                time = 0f;
            }
        }
    }

    // Giriş ekranında kapkaranlık kan kırmızısı arka plan:
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.SetUpRoleText))]
    public static class IntroPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            var seriKatil = PlayerControl.LocalPlayer.GetCustomRole<SeriKatilRole>();
            if (seriKatil != null)
            {
                Color kanKirmizisi = new Color(0.5f, 0.0f, 0.0f, 1f);
                if (__instance.BackgroundBar != null)
                {
                    __instance.BackgroundBar.material.color = kanKirmizisi;
                }
                __instance.RoleText.text = "SERİ KATİL";
                __instance.RoleBlurbText.text = "3 Kill hakkın var, bekleme süren YOK!";
            }
        }
    }
}
