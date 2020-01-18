open System
open System.Windows.Forms
open System.Drawing

[<EntryPoint>]
let main argv =
    Application.SetCompatibleTextRenderingDefault(false)
    Application.SetHighDpiMode(HighDpiMode.SystemAware) |> ignore
    Application.EnableVisualStyles()

    let f = new Form()
    f.BackColor <- Color.Black
    f.Bounds <- Screen.PrimaryScreen.Bounds
    f.FormBorderStyle <- FormBorderStyle.None
    f.TopMost <- true

    let loading = new Label()
    loading.Text <- "LOADING"
    loading.Font <- new Font("Bahnschrift SemiBold", float32 48)
    loading.ForeColor <- Color.FromArgb(255, 240, 240, 240)
    loading.AutoSize <- true
    loading.Location <- Point(f.Bounds.Width - loading.PreferredWidth, f.Bounds.Height - loading.PreferredHeight)

    let loadingFade = async {
        let grays = 
            Seq.unfold (fun (up, cur) -> 
                if cur > 240 then Some (240, (false, 240))
                else if cur < 60 then Some (60, (true, 60))
                else if up then Some (cur + 1, (up, cur + 1))
                else Some (cur - 1, (up, cur - 1)))
                (false, 240)
        for gray in grays do
            do! Async.Sleep(15)
            loading.ForeColor <- Color.FromArgb(255, gray, gray, gray)
    }

    let signal = new Label()
    signal.Text <- "No Signal"
    signal.Font <- new Font("Consolas", float32 52, FontStyle.Bold)
    signal.ForeColor <- Color.White
    signal.BackColor <- Color.Blue
    signal.AutoSize <- true
    signal.Location <- Point((f.Bounds.Width - signal.PreferredWidth) / 2, (f.Bounds.Height - signal.PreferredHeight) / 2)

    let signalJump = async {
        let mutable up = true
        let rand = Random()
        while true do
            let diff = rand.Next(50, 350)
            do! Async.Sleep(rand.Next(1000, 5000))
            let y = signal.Location.Y
            if (y - diff < 0) then up <- false 
            else if (y + diff + signal.PreferredHeight > f.Bounds.Height) then up <- true
            if up then signal.Location <- Point(signal.Location.X, y - diff)
            else signal.Location <- Point(signal.Location.X, y + diff)
    }

    f.KeyDown.Add <| fun event ->
        match event.KeyCode with
        | Keys.Escape -> Application.Exit()
        | Keys.Q -> 
            f.Controls.Clear()
            f.Controls.Add(signal)
            f.BackColor <- Color.DarkBlue
        | Keys.W ->
            f.Controls.Clear()
            f.Controls.Add(loading)
            f.BackColor <- Color.Black
        | Keys.E ->
            f.Controls.Clear()
            f.BackColor <- Color.Black
        | _ -> ()

    loadingFade |> Async.Start
    signalJump |> Async.Start
    Cursor.Hide()
    Application.Run(f)
    0
