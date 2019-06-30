﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using CheckPoint.Model;
using CheckPoint.Model.CameraAndPhoto;
using CheckPoint.Model.Entities;
using CheckPoint.Model.Repository;
using CheckPoint.View.Interfaces;

namespace CheckPoint.Presenter
{
    class MainFormPresenter
    {
        private readonly IMessageService _messageService;
        private readonly ApplicationContext _context;
        private readonly WebCamera _webCamera;
        private readonly Timer _cameraTimer;
        public IMainForm View { get; }

        public event EventHandler SettingsFormShow;

        public MainFormPresenter(IMainForm mainForm, IMessageService messageService, ApplicationContext context)
        {
            View = mainForm;
            _messageService = messageService;
            _context = context;
            _webCamera = new WebCamera();
            // show web-camera image
            _cameraTimer = new Timer((obj) => View.Camera = _webCamera.GetImage(),
                null, 500, 50);

            View.EmployeeChecked += _view_EmployeeChecked;
            View.CheckFormClick += _view_CheckFormClick;
            View.ReportsClick += _view_ReportsClick;
            View.SettingsClick += _view_SettingsClick;
            View.OnFormShow += ViewOnFormShow;
            View.OnFormClose += ViewOnFormClose;
        }

        private void ViewOnFormClose(object sender, EventArgs e)
        {
            if (_messageService.ShowQuestion("Close program?"))
            {
                _context.Dispose();
                View.CloseForm();
            }
        }

        private void ViewOnFormShow(object sender, EventArgs e)
        {

        }

        private void _view_SettingsClick(object sender, EventArgs e)
        {
            SettingsFormShow?.Invoke(sender, EventArgs.Empty);
        }

        private void _view_ReportsClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _view_CheckFormClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _view_EmployeeChecked(object sender, EventArgs e)
        {
            if (View.BarCode == string.Empty)
                return;

            var employeeCheck = new EmployeeCheck(View.IsEntry, View.BarCode, _context);
            var shiftCheck = employeeCheck.Check();
            ShowLastCheck(shiftCheck);
        }

        private void ShowLastCheck(ShiftCheck shiftCheck)
        {
            View.FullName = shiftCheck.Employee.FullName;
            View.Post = shiftCheck.Employee.Post.Name;
            View.DateTimeEntry = shiftCheck.DateTimeEntry;
            View.DateTimeExit = shiftCheck.DateTimeExit;
            _webCamera.SaveImageToFile(Path.Combine(Properties.Settings.Default.CheckPhotoFolder, shiftCheck.Employee.FullName + ".jpg"));
            View.CheckPhoto = _webCamera.Snapshot;
        }
    }
}