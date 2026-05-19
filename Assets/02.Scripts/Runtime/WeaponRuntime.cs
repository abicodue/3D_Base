public class WeaponRuntime
{
    public WeaponData data;
    public int currentAmmo;

    // 생성자 - 초기화 - 중요!
    public WeaponRuntime(WeaponData data)
    {
        this.data = data;

        if (data.useAmmo)
            currentAmmo = data.magazineSize;
        else
            currentAmmo = 0;
    }
    
    // 간이 기능은 일부러 논리적으로 게임이 망가지는 값을 넣어두는게 국룰?
    public bool HasAmmo()
    {
        if (!data.useAmmo)
            return true;

        return currentAmmo > 0;
    }

    public void ConsumeAmmo()
    {
        if (!data.useAmmo)
            return;

        currentAmmo--;
    }

    public void Reload()
    {
        if (!data.useAmmo)
            return;

        currentAmmo = data.magazineSize;
    }


}
