using Metin2Bot.Metin2Oficial;
using Metin2Bot.Screenshots;
using Metin2Bot.Singletons;

namespace Metin2Bot.Operaciones
{
    public static class Cliente
    {

        public static async Task CambiarCH(Metin2 metin, int channel)
        {
            await Salir(metin);

            var estaEnPantallaLogin = AccionesImg.PicLogin.ProcessText(metin);

            if (!estaEnPantallaLogin)
            {
                throw new Exception("Se esperaba que este en pantalla de login");
            }

            await User.ClickAt(
                metin.StartX + Resolution.ClickOtroServer().X,
                metin.StartY + Resolution.ClickOtroServer().Y,
                100);

            await User.ClickAt(
                metin.StartX + Resolution.ClickIberia().X,
                metin.StartY + Resolution.ClickIberia().Y,
                100);

            await User.ClickAt(
                metin.StartX + Resolution.ClickChannel(channel).X,
                metin.StartY + Resolution.ClickChannel(channel).Y,
                100);

            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();

            await Task.Delay(TimeSpan.FromSeconds(10));

            var estaEnChampSelect = AccionesImg.PicChampSelect.ProcessText(metin);

            if (!estaEnChampSelect)
            {
                throw new Exception("Se esperaba que este en pantalla de seleccion de campeon");
            }

            await Task.Delay(100);
            await MetinKeyboard.Instance.ApretarEnter();
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        public static async Task Salir(Metin2 metin)
        {
            await User.ClickAt(
                metin.StartX + Resolution.ClickESC().X,
                metin.StartY + Resolution.ClickESC().Y,
                50);

            await Task.Delay(200);

            await User.ClickAt(
                metin.StartX + Resolution.ClickSalir().X,
                metin.StartY + Resolution.ClickSalir().Y,
                50);

            await Task.Delay(TimeSpan.FromSeconds(11));
        }
    }
}
