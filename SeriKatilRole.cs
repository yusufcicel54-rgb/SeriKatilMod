using UnityEngine;
using MiraAPI.Roles;
using MiraAPI.Utilities;

namespace SeriKatilMod
{
    public class SeriKatilRole : CustomRole
    {
        public override string RoleName => "Seri Katil";
        public override Color RoleColor => new Color(0.85f, 0.05f, 0.05f); // Kan Kırmızısı
        public override RoleTeam Team => RoleTeam.Impostor;
        public override bool CanKill => true;

        // 3 Tane Kill Mermimiz Var!
        public int KalanMermi = 3;

        public override void OnRoleAssigned()
        {
            KalanMermi = 3;
            // Oyun başlar başlamaz kill süresini SIFIRLA!
            PlayerControl.LocalPlayer.SetKillTimer(0f);
            
            HudHelper.ShowNotification("Rolün: SERİ KATİL! 3 Kill hakkın var ve BEKLEME SÜRESİ YOK! Yapıştır!");
        }

        // Adam kestiğinde çalışan fonksiyon
        public void BiriniKestim(PlayerControl katil)
        {
            KalanMermi--;

            if (KalanMermi > 0)
            {
                // Mermi varsa bekleme süresini ANINDA SIFIRLA!
                katil.SetKillTimer(0f);
                HudHelper.ShowNotification($"PAT! Kalan Kill Hakkı: {KalanMermi} (BEKLEME SÜRESİ YOK, VUR!)");
            }
            else
            {
                // Mermi bitti! Bir daha kill atamasın diye süreyi 9999 saniye yapıyoruz
                katil.SetKillTimer(9999f);
                HudHelper.ShowNotification("ŞARJÖR BİTTİ! 3 kişiyi indirdin, artık görevini tamamladın!");
            }
        }
    }
}
