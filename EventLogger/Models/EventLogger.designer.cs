﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventLogger.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PilotDB")]
	public partial class EventLoggerDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertApp(App instance);
    partial void UpdateApp(App instance);
    partial void DeleteApp(App instance);
    partial void InsertEventType(EventType instance);
    partial void UpdateEventType(EventType instance);
    partial void DeleteEventType(EventType instance);
    partial void InsertEvent(Event instance);
    partial void UpdateEvent(Event instance);
    partial void DeleteEvent(Event instance);
    partial void InsertEvent_Aggregate(Event_Aggregate instance);
    partial void UpdateEvent_Aggregate(Event_Aggregate instance);
    partial void DeleteEvent_Aggregate(Event_Aggregate instance);
    #endregion
		
		public EventLoggerDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PilotDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public EventLoggerDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EventLoggerDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EventLoggerDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EventLoggerDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<App> Apps
		{
			get
			{
				return this.GetTable<App>();
			}
		}
		
		public System.Data.Linq.Table<EventType> EventTypes
		{
			get
			{
				return this.GetTable<EventType>();
			}
		}
		
		public System.Data.Linq.Table<Event> Events
		{
			get
			{
				return this.GetTable<Event>();
			}
		}
		
		public System.Data.Linq.Table<Event_Aggregate> Event_Aggregates
		{
			get
			{
				return this.GetTable<Event_Aggregate>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.App")]
	public partial class App : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private EntitySet<Event> _Events;
		
		private EntitySet<Event_Aggregate> _Event_Aggregates;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    #endregion
		
		public App()
		{
			this._Events = new EntitySet<Event>(new Action<Event>(this.attach_Events), new Action<Event>(this.detach_Events));
			this._Event_Aggregates = new EntitySet<Event_Aggregate>(new Action<Event_Aggregate>(this.attach_Event_Aggregates), new Action<Event_Aggregate>(this.detach_Event_Aggregates));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="App_Event", Storage="_Events", ThisKey="Id", OtherKey="App_Id")]
		public EntitySet<Event> Events
		{
			get
			{
				return this._Events;
			}
			set
			{
				this._Events.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="App_Event_Aggregate", Storage="_Event_Aggregates", ThisKey="Id", OtherKey="App_Id")]
		public EntitySet<Event_Aggregate> Event_Aggregates
		{
			get
			{
				return this._Event_Aggregates;
			}
			set
			{
				this._Event_Aggregates.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Events(Event entity)
		{
			this.SendPropertyChanging();
			entity.App = this;
		}
		
		private void detach_Events(Event entity)
		{
			this.SendPropertyChanging();
			entity.App = null;
		}
		
		private void attach_Event_Aggregates(Event_Aggregate entity)
		{
			this.SendPropertyChanging();
			entity.App = this;
		}
		
		private void detach_Event_Aggregates(Event_Aggregate entity)
		{
			this.SendPropertyChanging();
			entity.App = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.EventType")]
	public partial class EventType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Name;
		
		private bool _Reportable;
		
		private EntitySet<Event> _Events;
		
		private EntitySet<Event_Aggregate> _Event_Aggregates;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnReportableChanging(bool value);
    partial void OnReportableChanged();
    #endregion
		
		public EventType()
		{
			this._Events = new EntitySet<Event>(new Action<Event>(this.attach_Events), new Action<Event>(this.detach_Events));
			this._Event_Aggregates = new EntitySet<Event_Aggregate>(new Action<Event_Aggregate>(this.attach_Event_Aggregates), new Action<Event_Aggregate>(this.detach_Event_Aggregates));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Reportable", DbType="Bit NOT NULL")]
		public bool Reportable
		{
			get
			{
				return this._Reportable;
			}
			set
			{
				if ((this._Reportable != value))
				{
					this.OnReportableChanging(value);
					this.SendPropertyChanging();
					this._Reportable = value;
					this.SendPropertyChanged("Reportable");
					this.OnReportableChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="EventType_Event", Storage="_Events", ThisKey="Id", OtherKey="EventType_Id")]
		public EntitySet<Event> Events
		{
			get
			{
				return this._Events;
			}
			set
			{
				this._Events.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="EventType_Event_Aggregate", Storage="_Event_Aggregates", ThisKey="Id", OtherKey="EventType_Id")]
		public EntitySet<Event_Aggregate> Event_Aggregates
		{
			get
			{
				return this._Event_Aggregates;
			}
			set
			{
				this._Event_Aggregates.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Events(Event entity)
		{
			this.SendPropertyChanging();
			entity.EventType = this;
		}
		
		private void detach_Events(Event entity)
		{
			this.SendPropertyChanging();
			entity.EventType = null;
		}
		
		private void attach_Event_Aggregates(Event_Aggregate entity)
		{
			this.SendPropertyChanging();
			entity.EventType = this;
		}
		
		private void detach_Event_Aggregates(Event_Aggregate entity)
		{
			this.SendPropertyChanging();
			entity.EventType = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Event")]
	public partial class Event : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private System.DateTime _CreatedAt;
		
		private int _EventType_Id;
		
		private System.Nullable<int> _App_Id;
		
		private EntityRef<App> _App;
		
		private EntityRef<EventType> _EventType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnCreatedAtChanging(System.DateTime value);
    partial void OnCreatedAtChanged();
    partial void OnEventType_IdChanging(int value);
    partial void OnEventType_IdChanged();
    partial void OnApp_IdChanging(System.Nullable<int> value);
    partial void OnApp_IdChanged();
    #endregion
		
		public Event()
		{
			this._App = default(EntityRef<App>);
			this._EventType = default(EntityRef<EventType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedAt", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedAt
		{
			get
			{
				return this._CreatedAt;
			}
			set
			{
				if ((this._CreatedAt != value))
				{
					this.OnCreatedAtChanging(value);
					this.SendPropertyChanging();
					this._CreatedAt = value;
					this.SendPropertyChanged("CreatedAt");
					this.OnCreatedAtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EventType_Id", DbType="Int NOT NULL")]
		public int EventType_Id
		{
			get
			{
				return this._EventType_Id;
			}
			set
			{
				if ((this._EventType_Id != value))
				{
					if (this._EventType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnEventType_IdChanging(value);
					this.SendPropertyChanging();
					this._EventType_Id = value;
					this.SendPropertyChanged("EventType_Id");
					this.OnEventType_IdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_App_Id", DbType="Int")]
		public System.Nullable<int> App_Id
		{
			get
			{
				return this._App_Id;
			}
			set
			{
				if ((this._App_Id != value))
				{
					if (this._App.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnApp_IdChanging(value);
					this.SendPropertyChanging();
					this._App_Id = value;
					this.SendPropertyChanged("App_Id");
					this.OnApp_IdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="App_Event", Storage="_App", ThisKey="App_Id", OtherKey="Id", IsForeignKey=true)]
		public App App
		{
			get
			{
				return this._App.Entity;
			}
			set
			{
				App previousValue = this._App.Entity;
				if (((previousValue != value) 
							|| (this._App.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._App.Entity = null;
						previousValue.Events.Remove(this);
					}
					this._App.Entity = value;
					if ((value != null))
					{
						value.Events.Add(this);
						this._App_Id = value.Id;
					}
					else
					{
						this._App_Id = default(Nullable<int>);
					}
					this.SendPropertyChanged("App");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="EventType_Event", Storage="_EventType", ThisKey="EventType_Id", OtherKey="Id", IsForeignKey=true)]
		public EventType EventType
		{
			get
			{
				return this._EventType.Entity;
			}
			set
			{
				EventType previousValue = this._EventType.Entity;
				if (((previousValue != value) 
							|| (this._EventType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._EventType.Entity = null;
						previousValue.Events.Remove(this);
					}
					this._EventType.Entity = value;
					if ((value != null))
					{
						value.Events.Add(this);
						this._EventType_Id = value.Id;
					}
					else
					{
						this._EventType_Id = default(int);
					}
					this.SendPropertyChanged("EventType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Event_Aggregate")]
	public partial class Event_Aggregate : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _EventType_Id;
		
		private System.Nullable<int> _App_Id;
		
		private int _Count;
		
		private System.DateTime _Date;
		
		private EntityRef<App> _App;
		
		private EntityRef<EventType> _EventType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnEventType_IdChanging(int value);
    partial void OnEventType_IdChanged();
    partial void OnApp_IdChanging(System.Nullable<int> value);
    partial void OnApp_IdChanged();
    partial void OnCountChanging(int value);
    partial void OnCountChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    #endregion
		
		public Event_Aggregate()
		{
			this._App = default(EntityRef<App>);
			this._EventType = default(EntityRef<EventType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EventType_Id", DbType="Int NOT NULL")]
		public int EventType_Id
		{
			get
			{
				return this._EventType_Id;
			}
			set
			{
				if ((this._EventType_Id != value))
				{
					if (this._EventType.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnEventType_IdChanging(value);
					this.SendPropertyChanging();
					this._EventType_Id = value;
					this.SendPropertyChanged("EventType_Id");
					this.OnEventType_IdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_App_Id", DbType="Int")]
		public System.Nullable<int> App_Id
		{
			get
			{
				return this._App_Id;
			}
			set
			{
				if ((this._App_Id != value))
				{
					if (this._App.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnApp_IdChanging(value);
					this.SendPropertyChanging();
					this._App_Id = value;
					this.SendPropertyChanged("App_Id");
					this.OnApp_IdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Count", DbType="Int NOT NULL")]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				if ((this._Count != value))
				{
					this.OnCountChanging(value);
					this.SendPropertyChanging();
					this._Count = value;
					this.SendPropertyChanged("Count");
					this.OnCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="Date NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="App_Event_Aggregate", Storage="_App", ThisKey="App_Id", OtherKey="Id", IsForeignKey=true)]
		public App App
		{
			get
			{
				return this._App.Entity;
			}
			set
			{
				App previousValue = this._App.Entity;
				if (((previousValue != value) 
							|| (this._App.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._App.Entity = null;
						previousValue.Event_Aggregates.Remove(this);
					}
					this._App.Entity = value;
					if ((value != null))
					{
						value.Event_Aggregates.Add(this);
						this._App_Id = value.Id;
					}
					else
					{
						this._App_Id = default(Nullable<int>);
					}
					this.SendPropertyChanged("App");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="EventType_Event_Aggregate", Storage="_EventType", ThisKey="EventType_Id", OtherKey="Id", IsForeignKey=true)]
		public EventType EventType
		{
			get
			{
				return this._EventType.Entity;
			}
			set
			{
				EventType previousValue = this._EventType.Entity;
				if (((previousValue != value) 
							|| (this._EventType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._EventType.Entity = null;
						previousValue.Event_Aggregates.Remove(this);
					}
					this._EventType.Entity = value;
					if ((value != null))
					{
						value.Event_Aggregates.Add(this);
						this._EventType_Id = value.Id;
					}
					else
					{
						this._EventType_Id = default(int);
					}
					this.SendPropertyChanged("EventType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
