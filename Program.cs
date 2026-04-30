using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using System.Collections.ObjectModel;

// (Keep all your other classes exactly as they are)

public class Program
{
    private static AbstractWorker? _currentWorker;
    private static List<AbstractWorker> _workerList = new();
    
    // FIX: Changed from ListView to TextView to support your \n formatting!
    private static TextView? _logTextView;

    public static void Main()
    {
        InitializeDummyWarehouse();

        _workerList.Add(new ArrivalManager(1, "arr", 5000, "arr", "123"));
        _workerList.Add(new InnerManager(2, "inn", 5000, "inn", "123"));
        _workerList.Add(new DepartureManager(3, "dep", 5000, "dep", "123"));

        using IApplication app = Application.Create();
        app.Init();

        bool isRunning = true;
        
        while (isRunning)
        {
            if (_currentWorker == null)
            {
                bool quitRequested = ShowLoginDialog(app);
                if (quitRequested) break; 
            }

            if (_currentWorker != null)
            {
                bool logoutRequested = ShowDashboardWindow(app);
                if (!logoutRequested) break; 
            }
        }
    }

    private static void InitializeDummyWarehouse()
    {
        StorageController.Core.ItemArray = new List<Zone>();
        ExpandStorageToAddress(new Address { Zone=0, Section=0, Rack=0, Shelf=0, Cell=0 }, 100);
        ExpandStorageToAddress(new Address { Zone=0, Section=0, Rack=0, Shelf=0, Cell=1 }, 100);
    }

    private static bool ShowLoginDialog(IApplication app)
    {
        bool quit = false;
        var dialog = new Dialog() { Title = "Warehouse System - Login", Width = 40, Height = 10 };

        var lblUser = new Label() { Text = "Username:", X = 1, Y = 1 };
        var txtUser = new TextField() { X = Pos.Right(lblUser) + 1, Y = 1, Width = 20 };

        var lblPass = new Label() { Text = "Password:", X = 1, Y = 3 };
        var txtPass = new TextField() { X = Pos.Right(lblPass) + 1, Y = 3, Width = 20, Secret = true };

        var btnLogin = new Button() { Text = "Login", IsDefault = true };
        var btnRegister = new Button() { Text = "Register" };
        var btnQuit = new Button() { Text = "Quit" };

        btnLogin.Accepting += (s, e) =>
        {
            var worker = _workerList.Find(w => w.Name == (txtUser.Text ?? ""));
            if (worker != null && worker.LoginAccount(txtPass.Text ?? ""))
            {
                _currentWorker = worker; 
                app.RequestStop(); 
            }
            else
            {
                MessageBox.ErrorQuery(app, "Error", "Invalid Credentials!", "Ok");
            }
        };

        btnRegister.Accepting += (s, e) => ShowRegistrationDialog(app);
        btnQuit.Accepting += (s, e) => { quit = true; app.RequestStop(); };

        dialog.Add(lblUser, txtUser, lblPass, txtPass);
        dialog.AddButton(btnLogin);
        dialog.AddButton(btnRegister);
        dialog.AddButton(btnQuit);

        app.Run(dialog);
        return quit;
    }

    private static void ShowRegistrationDialog(IApplication app)
    {
        var dialog = new Dialog() { Title = "Register New Worker", Width = 50, Height = 16 };

        var lblRole = new Label() { Text = "Role:", X = 1, Y = 1 };
        var listRole = new ListView() { X = Pos.Right(lblRole) + 1, Y = 1, Width = 25, Height = 3 };
        listRole.SetSource(new ObservableCollection<string>(new List<string> { "Arrival Manager", "Inner Manager", "Departure Manager" }));

        var lblName = new Label() { Text = "Name:", X = 1, Y = 5 };
        var txtName = new TextField() { X = Pos.Right(lblName) + 1, Y = 5, Width = 20 };

        var lblPass = new Label() { Text = "Password:", X = 1, Y = 7 };
        var txtPass = new TextField() { X = Pos.Right(lblPass) + 1, Y = 7, Width = 20, Secret = true };

        var btnSave = new Button() { Text = "Save", X = Pos.Center(), Y = Pos.Bottom(txtPass) + 2 };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSave.Accepting += (s, e) =>
        {
            string nameInput = txtName.Text ?? "";
            string passInput = txtPass.Text ?? "";

            if (string.IsNullOrWhiteSpace(nameInput))
            {
                MessageBox.ErrorQuery(app, "Error", "Name cannot be empty.", "Ok");
                return;
            }

            int id = new Random().Next(100, 999);
            AbstractWorker newWorker = listRole.SelectedItem switch
            {
                0 => new ArrivalManager(id, nameInput, 3000, nameInput, passInput),
                1 => new InnerManager(id, nameInput, 3500, nameInput, passInput),
                _ => new DepartureManager(id, nameInput, 4000, nameInput, passInput)
            };

            _workerList.Add(newWorker);
            MessageBox.Query(app, "Success", $"{newWorker.GetType().Name} Registered!", "Ok");
            app.RequestStop(); 
        };

        btnCancel.Accepting += (s,e) => app.RequestStop();
        dialog.Add(lblRole, listRole, lblName, txtName, lblPass, txtPass);
        dialog.AddButton(btnSave); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static bool ShowDashboardWindow(IApplication app)
    {
        bool loggingOut = false;
        
        using Window mainWin = new Window()
        {
            Title = $"Dashboard - Logged in as: {_currentWorker?.Name} ({_currentWorker?.GetType().Name})",
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Logout", "", () => { loggingOut = true; _currentWorker = null; app.RequestStop(); }),
                new MenuItem ("_Quit", "", () => { loggingOut = false; app.RequestStop(); })
            })
        });

        var controlsFrame = new FrameView() { Title = "Actions", X = 0, Y = 1, Width = Dim.Percent(30), Height = Dim.Fill() };

        var btnAdd = new Button() { Text = "Add Cargo", X = 1, Y = 1 };
        var btnMove = new Button() { Text = "Move Cargo", X = 1, Y = 3 };
        var btnRemove = new Button() { Text = "Remove Cargo", X = 1, Y = 5 };
        var btnEdit = new Button() { Text = "Edit Cargo", X = 1, Y = 7 }; 
        var btnAddStorage = new Button() { Text = "Create Storage Cell", X = 1, Y = 9 };
        
        var btnFindEmpty = new Button() { Text = "Find Empty Cell", X = 1, Y = 12 };
        var btnCheckCell = new Button() { Text = "Check Cell Content", X = 1, Y = 14 };
        var btnViewLayout = new Button() { Text = "View Full Layout", X = 1, Y = 16 };
        
        var btnFilterLogs = new Button() { Text = "Filter Logs by Date", X = 1, Y = 19 };
        var btnRefreshLogs = new Button() { Text = "Show All Logs", X = 1, Y = 21 };

        btnAdd.Accepting += (s, e) => ShowAddCargoDialog(app);
        btnMove.Accepting += (s, e) => ShowMoveCargoDialog(app);
        btnRemove.Accepting += (s, e) => ShowRemoveCargoDialog(app);
        btnEdit.Accepting += (s, e) => ShowEditCargoDialog(app);
        btnAddStorage.Accepting += (s, e) => ShowAddStorageDialog(app);
        
        btnFindEmpty.Accepting += (s, e) => HandleFindEmptyCell(app);
        btnCheckCell.Accepting += (s, e) => ShowCheckCellDialog(app);
        btnViewLayout.Accepting += (s, e) => ShowStorageLayoutDialog(app);

        btnFilterLogs.Accepting += (s, e) => ShowFilterLogsDialog(app);
        btnRefreshLogs.Accepting += (s, e) => SyncLogs();

        controlsFrame.Add(btnAdd, btnMove, btnRemove, btnEdit, btnAddStorage, btnFindEmpty, btnCheckCell, btnViewLayout, btnFilterLogs, btnRefreshLogs);

        var logFrame = new FrameView() { Title = "Warehouse Transcripts", X = Pos.Right(controlsFrame), Y = 1, Width = Dim.Fill(), Height = Dim.Fill() };

        // FIX: Using TextView so it fully supports \n and multi-line formatting!
        _logTextView = new TextView() 
        { 
            X = 0, Y = 0, 
            Width = Dim.Fill(), Height = Dim.Fill(), 
            ReadOnly = true,
            WordWrap = true
        };
        logFrame.Add(_logTextView);

        SyncLogs();

        mainWin.Add(menu, controlsFrame, logFrame);
        app.Run(mainWin);
        return loggingOut;
    }

    private static void ShowAddCargoDialog(IApplication app)
    {
        if (_currentWorker is not ArrivalManager am)
        {
            MessageBox.ErrorQuery(app, "Access Denied", "Only Arrival Managers can add items.", "Ok");
            return;
        }

        var dialog = new Dialog() { Title = "Add New Cargo", Width = 50, Height = 20 };

        var lblName = new Label() { Text = "Name:", X = 1, Y = 1 };
        var txtName = new TextField() { X = Pos.Right(lblName) + 1, Y = 1, Width = 25 };
        var lblSize = new Label() { Text = "Size:", X = 1, Y = 3 };
        var txtSize = new TextField() { X = Pos.Right(lblSize) + 1, Y = 3, Width = 10 };
        var lblSource = new Label() { Text = "Source:", X = 1, Y = 5 };
        var txtSource = new TextField() { X = Pos.Right(lblSource) + 1, Y = 5, Width = 25 };
        var lblDest = new Label() { Text = "Destination:", X = 1, Y = 7 };
        var txtDest = new TextField() { X = Pos.Right(lblDest) + 1, Y = 7, Width = 25 };
        var lblDesc = new Label() { Text = "Description:", X = 1, Y = 9 };
        var txtDesc = new TextField() { X = Pos.Right(lblDesc) + 1, Y = 9, Width = 25 };
        
        var lblAddr = new Label() { Text = "Address (Z-S-R-S-C):", X = 1, Y = 11 };
        var txtAddr = new TextField() { Text = StorageController.FindEmptyCell()?.ToString() ?? "0-0-0-0-0", X = Pos.Right(lblAddr) + 1, Y = 11, Width = 15 };

        var btnSubmit = new Button() { Text = "Submit", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            try
            {
                Cargo newCargo = new Cargo(txtName.Text ?? "Unknown", int.Parse(txtSize.Text ?? "0"), txtSource.Text ?? "Unknown", txtDest.Text ?? "Unknown", txtDesc.Text ?? "None");
                am.AddItem(ParseAddress(txtAddr.Text ?? ""), newCargo);
                SyncLogs();
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Input Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblName, txtName, lblSize, txtSize, lblSource, txtSource, lblDest, txtDest, lblDesc, txtDesc, lblAddr, txtAddr);
        dialog.AddButton(btnSubmit); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void ShowMoveCargoDialog(IApplication app)
    {
        if (_currentWorker is not InnerManager im)
        {
            MessageBox.ErrorQuery(app, "Access Denied", "Not authorized to move items.", "Ok");
            return;
        }

        var dialog = new Dialog() { Title = "Move Cargo", Width = 50, Height = 12 };
        var lblFrom = new Label() { Text = "From (Z-S-R-S-C):", X = 1, Y = 1 };
        var txtFrom = new TextField() { Text = "0-0-0-0-0", X = Pos.Right(lblFrom) + 1, Y = 1, Width = 15 };
        var lblTo = new Label() { Text = "To (Z-S-R-S-C):", X = 1, Y = 3 };
        var txtTo = new TextField() { Text = "0-0-0-0-1", X = Pos.Right(lblTo) + 1, Y = 3, Width = 15 };

        var btnSubmit = new Button() { Text = "Submit", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            try
            {
                im.MoveItem(ParseAddress(txtFrom.Text ?? ""), ParseAddress(txtTo.Text ?? ""));
                SyncLogs();
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Move Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblFrom, txtFrom, lblTo, txtTo);
        dialog.AddButton(btnSubmit); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void ShowRemoveCargoDialog(IApplication app)
    {
        if (_currentWorker is not DepartureManager dm)
        {
            MessageBox.ErrorQuery(app, "Access Denied", "Only Departure Managers can remove items.", "Ok");
            return;
        }

        var dialog = new Dialog() { Title = "Remove Cargo", Width = 50, Height = 10 };
        var lblAddr = new Label() { Text = "Address (Z-S-R-S-C):", X = 1, Y = 1 };
        var txtAddr = new TextField() { Text = "0-0-0-0-0", X = Pos.Right(lblAddr) + 1, Y = 1, Width = 15 };

        var btnSubmit = new Button() { Text = "Submit", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            try
            {
                dm.RemoveItem(ParseAddress(txtAddr.Text ?? ""));
                SyncLogs();
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Input Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblAddr, txtAddr);
        dialog.AddButton(btnSubmit); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void ShowEditCargoDialog(IApplication app)
    {
        if (_currentWorker is not InnerManager)
        {
            MessageBox.ErrorQuery(app, "Access Denied", "Only Managers can edit cargo manifests.", "Ok");
            return;
        }

        var dialog = new Dialog() { Title = "Edit Cargo - Select Cell", Width = 50, Height = 10 };
        var lblAddr = new Label() { Text = "Address (Z-S-R-S-C):", X = 1, Y = 1 };
        var txtAddr = new TextField() { Text = "0-0-0-0-0", X = Pos.Right(lblAddr) + 1, Y = 1, Width = 15 };

        var btnLoad = new Button() { Text = "Load Cargo", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnLoad.Accepting += (s, e) =>
        {
            try
            {
                Address address = ParseAddress(txtAddr.Text ?? "");
                Cell? cell = StorageController.CheckCell(address);

                if (cell == null || cell.IsEmpty || cell.Item == null) throw new Exception("Cell is empty! Nothing to edit.");

                app.RequestStop();
                OpenCargoEditForm(app, cell, address);
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Search Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblAddr, txtAddr);
        dialog.AddButton(btnLoad); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void OpenCargoEditForm(IApplication app, Cell cell, Address address)
    {
        Cargo cargo = cell.Item!; 

        var dialog = new Dialog() { Title = $"Editing Cargo at {address}", Width = 50, Height = 20 };

        var lblName = new Label() { Text = "Name:", X = 1, Y = 1 };
        var txtName = new TextField() { Text = cargo.Name, X = Pos.Right(lblName) + 1, Y = 1, Width = 25 };
        var lblSize = new Label() { Text = "Size:", X = 1, Y = 3 };
        var txtSize = new TextField() { Text = cargo.Size.ToString(), X = Pos.Right(lblSize) + 1, Y = 3, Width = 10 };
        var lblSource = new Label() { Text = "Source:", X = 1, Y = 5 };
        var txtSource = new TextField() { Text = cargo.Source, X = Pos.Right(lblSource) + 1, Y = 5, Width = 25 };
        var lblDest = new Label() { Text = "Destination:", X = 1, Y = 7 };
        var txtDest = new TextField() { Text = cargo.Destination, X = Pos.Right(lblDest) + 1, Y = 7, Width = 25 };
        var lblDesc = new Label() { Text = "Description:", X = 1, Y = 9 };
        var txtDesc = new TextField() { Text = cargo.Description, X = Pos.Right(lblDesc) + 1, Y = 9, Width = 25 };

        var btnSave = new Button() { Text = "Save Changes", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSave.Accepting += (s, e) =>
        {
            try
            {
                int newSize = int.Parse(txtSize.Text ?? "0");
                if (newSize > cell.Capacity) throw new Exception($"New size ({newSize}) exceeds capacity ({cell.Capacity}).");

                cargo.Name = txtName.Text ?? "Unknown";
                cargo.Size = newSize;
                cargo.Source = txtSource.Text ?? "Unknown";
                cargo.Destination = txtDest.Text ?? "Unknown";
                cargo.Description = txtDesc.Text ?? "None";

                MessageBox.Query(app, "Success", "Cargo updated successfully!", "Ok");
                
                // Add a native log entry for edits since it isn't an official Manager method yet!
                LogAction($"Worker {_currentWorker?.Name} edited cargo at {address}");
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Input Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblName, txtName, lblSize, txtSize, lblSource, txtSource, lblDest, txtDest, lblDesc, txtDesc);
        dialog.AddButton(btnSave); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void ShowAddStorageDialog(IApplication app)
    {
        if (_currentWorker is not InnerManager)
        {
            MessageBox.ErrorQuery(app, "Access Denied", "Managers required to modify warehouse layout.", "Ok");
            return;
        }

        var dialog = new Dialog() { Title = "Create New Storage Cell", Width = 50, Height = 14 };
        var lblAddr = new Label() { Text = "Address (Z-S-R-S-C):", X = 1, Y = 1 };
        var txtAddr = new TextField() { Text = "0-0-0-0-2", X = Pos.Right(lblAddr) + 1, Y = 1, Width = 15 };
        var lblCap = new Label() { Text = "Capacity:", X = 1, Y = 3 };
        var txtCap = new TextField() { Text = "100", X = Pos.Right(lblCap) + 1, Y = 3, Width = 15 };

        var btnSubmit = new Button() { Text = "Submit", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            try
            {
                ExpandStorageToAddress(ParseAddress(txtAddr.Text ?? ""), int.Parse(txtCap.Text ?? "100"));
                MessageBox.Query(app, "Created", "Storage cell created successfully.", "Ok");
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblAddr, txtAddr, lblCap, txtCap);
        dialog.AddButton(btnSubmit); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void HandleFindEmptyCell(IApplication app)
    {
        Address? emptyAddr = StorageController.FindEmptyCell();
        if (emptyAddr != null) MessageBox.Query(app, "Cell Found", $"The first available empty cell is at:\n\n{emptyAddr}", "Ok");
        else MessageBox.ErrorQuery(app, "Warehouse Full", "Could not find any empty cells.", "Ok");
    }

    private static void ShowCheckCellDialog(IApplication app)
    {
        var dialog = new Dialog() { Title = "Check Cell Content", Width = 50, Height = 10 };
        var lblAddr = new Label() { Text = "Address (Z-S-R-S-C):", X = 1, Y = 1 };
        var txtAddr = new TextField() { Text = "0-0-0-0-0", X = Pos.Right(lblAddr) + 1, Y = 1, Width = 15 };

        var btnSubmit = new Button() { Text = "Check", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            try
            {
                Address address = ParseAddress(txtAddr.Text ?? "");
                Cell? cell = StorageController.CheckCell(address);

                if (cell == null) MessageBox.ErrorQuery(app, "Not Found", $"No cell exists at {address}.", "Ok");
                else if (cell.IsEmpty) MessageBox.Query(app, "Cell Data", $"Status: EMPTY\nCapacity: {cell.Capacity}", "Ok");
                else MessageBox.Query(app, "Cell Data", $"Status: OCCUPIED\nCapacity: {cell.Capacity}\n\nCargo Manifest:\n{cell.Item}", "Ok");
                
                app.RequestStop();
            }
            catch (Exception ex) { MessageBox.ErrorQuery(app, "Input Error", ex.Message, "Ok"); }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();
        dialog.Add(lblAddr, txtAddr);
        dialog.AddButton(btnSubmit); dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    private static void ShowStorageLayoutDialog(IApplication app)
    {
        var dialog = new Dialog() { Title = "Warehouse Layout Hierarchy", Width = Dim.Percent(80), Height = Dim.Percent(80) };
        var textView = new TextView()
        {
            X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() - 2, ReadOnly = true,
            Text = StorageController.Core.ToString() ?? "Empty Warehouse"
        };

        var btnClose = new Button() { Text = "Close", IsDefault = true };
        btnClose.Accepting += (s, e) => app.RequestStop();

        dialog.Add(textView); dialog.AddButton(btnClose);
        app.Run(dialog);
    }

    private static void ShowFilterLogsDialog(IApplication app)
    {
        var dialog = new Dialog() { Title = "Filter Logs by Date", Width = 50, Height = 12 };

        var lblFrom = new Label() { Text = "From (yyyy-MM-dd):", X = 1, Y = 1 };
        var txtFrom = new TextField() { Text = DateTime.Today.ToString("yyyy-MM-dd"), X = Pos.Right(lblFrom) + 1, Y = 1, Width = 15 };

        var lblTo = new Label() { Text = "To (yyyy-MM-dd):", X = 1, Y = 3 };
        var txtTo = new TextField() { Text = DateTime.Today.ToString("yyyy-MM-dd"), X = Pos.Right(lblTo) + 1, Y = 3, Width = 15 };

        var btnSubmit = new Button() { Text = "Filter", IsDefault = true };
        var btnCancel = new Button() { Text = "Cancel" };

        btnSubmit.Accepting += (s, e) =>
        {
            if (DateTime.TryParse(txtFrom.Text ?? "", out DateTime fromDate) &&
                DateTime.TryParse(txtTo.Text ?? "", out DateTime toDate))
            {
                toDate = toDate.AddDays(1).AddTicks(-1);

                // Fetch filtered transcripts, map them natively, and assign directly to the TextView!
                var filteredLogs = Log.GetFromDate(fromDate, toDate).Select(t => t.ToString());
                _logTextView!.Text = string.Join("\n\n", filteredLogs);
                _logTextView.SetNeedsDraw();

                app.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery(app, "Date Error", "Invalid date format. Use yyyy-MM-dd.", "Ok");
            }
        };

        btnCancel.Accepting += (s, e) => app.RequestStop();

        dialog.Add(lblFrom, txtFrom, lblTo, txtTo);
        dialog.AddButton(btnSubmit);
        dialog.AddButton(btnCancel);
        app.Run(dialog);
    }

    // --- GUI HELPER METHODS ---

    private static void SyncLogs()
    {
        if (_logTextView == null) return;

        // Keep YOUR exact formatting, joined with double newlines for nice spacing!
        var allLogs = Log.GetAllTranscripts().Select(t => t.ToString());
        _logTextView.Text = string.Join("\n\n", allLogs);
        _logTextView.SetNeedsDraw();
    }

    private static void LogAction(string message)
    {
        if (_logTextView != null)
        {
            string newEntry = $"[{DateTime.Now:HH:mm:ss}] {message}\n\n";
            _logTextView.Text += newEntry;
            _logTextView.SetNeedsDraw();
        }
    }

    private static void ExpandStorageToAddress(Address addr, int capacity)
    {
        while (StorageController.Core.ItemArray.Count <= addr.Zone) StorageController.Core.ItemArray.Add(new Zone(new List<Section>(), $"Zone {StorageController.Core.ItemArray.Count}"));
        var zone = StorageController.Core.ItemArray[addr.Zone!.Value];

        while (zone.ItemArray.Count <= addr.Section) zone.ItemArray.Add(new Section(new List<Rack>(), $"Section {zone.ItemArray.Count}"));
        var section = zone.ItemArray[addr.Section!.Value];

        while (section.ItemArray.Count <= addr.Rack) section.ItemArray.Add(new Rack(new List<Shelf>(), $"Rack {section.ItemArray.Count}"));
        var rack = section.ItemArray[addr.Rack!.Value];

        while (rack.ItemArray.Count <= addr.Shelf) rack.ItemArray.Add(new Shelf(new List<Cell>(), $"Shelf {rack.ItemArray.Count}"));
        var shelf = rack.ItemArray[addr.Shelf!.Value];

        while (shelf.ItemArray.Count <= addr.Cell) shelf.ItemArray.Add(new Cell(0, new Address { Zone = addr.Zone, Section = addr.Section, Rack = addr.Rack, Shelf = addr.Shelf, Cell = shelf.ItemArray.Count }));
        shelf.ItemArray[addr.Cell!.Value].Capacity = capacity;
    }

    private static Address ParseAddress(string input)
    {
        var parts = input.Split('-');
        if (parts.Length != 5) throw new FormatException("Address must be in format Z-S-R-S-C");

        return new Address
        {
            Zone = int.Parse(parts[0]), Section = int.Parse(parts[1]),
            Rack = int.Parse(parts[2]), Shelf = int.Parse(parts[3]), Cell = int.Parse(parts[4])
        };
    }
}