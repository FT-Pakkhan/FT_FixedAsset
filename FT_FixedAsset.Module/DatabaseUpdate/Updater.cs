using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using FT_FixedAsset.Module.BusinessObjects;
using System;

namespace FT_FixedAsset.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            SystemUsers defaultUser = ObjectSpace.FindObject<SystemUsers>(new BinaryOperator("UserName", "fa"));
            if(defaultUser == null)
            {
                defaultUser = ObjectSpace.CreateObject<SystemUsers>();
                defaultUser.UserName = "fa";
                defaultUser.FullName = "Fixed Asset User";
                defaultUser.SetPassword("1234");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            defaultUser.Roles.Add(defaultRole);

            SystemUsers userAdmin = ObjectSpace.FindObject<SystemUsers>(new BinaryOperator("UserName", "sa"));
            if(userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<SystemUsers>();
                userAdmin.UserName = "sa";
                userAdmin.FullName = "sa";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("sa");
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if(adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.Roles.Add(adminRole);

            // Default General Settings Record
            FTS11OADM generalSettings = ObjectSpace.FindObject<FTS11OADM>(new BinaryOperator("DocEntry", "1"));
            if (generalSettings == null)
            {
                generalSettings = ObjectSpace.CreateObject<FTS11OADM>();
                generalSettings.CompnyName = "FAST TRACK FIXED ASSET";
            }

            // Default Transaction Type
            FTS11STRN trnsType0101 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "1"));
            if (trnsType0101 == null)
            {
                trnsType0101 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0101.Code = "T0101";
                trnsType0101.Name = "New Acquisition";
                trnsType0101.U_TrnsGrp = "IN";
                trnsType0101.U_Post = false;
            }

            FTS11STRN trnsType0102 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "2"));
            if (trnsType0102 == null)
            {
                trnsType0102 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0102.Code = "T0102";
                trnsType0102.Name = "Brought Foward";
                trnsType0102.U_TrnsGrp = "IN";
                trnsType0102.U_Post = false;
            }

            FTS11STRN trnsType0103 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "3"));
            if (trnsType0103 == null)
            {
                trnsType0103 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0103.Code = "T0103";
                trnsType0103.Name = "Correction In";
                trnsType0103.U_TrnsGrp = "IN";
                trnsType0103.U_Post = true;
            }

            FTS11STRN trnsType0104 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "4"));
            if (trnsType0104 == null)
            {
                trnsType0104 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0104.Code = "T0104";
                trnsType0104.Name = "Transferred In";
                trnsType0104.U_TrnsGrp = "IN";
                trnsType0104.U_Post = true;
            }

            FTS11STRN trnsType0105 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "5"));
            if (trnsType0105 == null)
            {
                trnsType0105 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0105.Code = "T0105";
                trnsType0105.Name = "Reclassed In";
                trnsType0105.U_TrnsGrp = "IN";
                trnsType0105.U_Post = true;
            }

            FTS11STRN trnsType0201 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "6"));
            if (trnsType0201 == null)
            {
                trnsType0201 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0201.Code = "T0201";
                trnsType0201.Name = "Write Off";
                trnsType0201.U_TrnsGrp = "OUT";
                trnsType0201.U_Post = true;
            }

            FTS11STRN trnsType0202 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "7"));
            if (trnsType0202 == null)
            {
                trnsType0202 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0202.Code = "T0202";
                trnsType0202.Name = "Disposed";
                trnsType0202.U_TrnsGrp = "OUT";
                trnsType0202.U_Post = true;
            }

            FTS11STRN trnsType0203 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "8"));
            if (trnsType0203 == null)
            {
                trnsType0203 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0203.Code = "T0203";
                trnsType0203.Name = "Correction Out";
                trnsType0203.U_TrnsGrp = "OUT";
                trnsType0203.U_Post = true;
            }

            FTS11STRN trnsType0204 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "9"));
            if (trnsType0204 == null)
            {
                trnsType0204 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0204.Code = "T0204";
                trnsType0204.Name = "Transferred Out";
                trnsType0204.U_TrnsGrp = "OUT";
                trnsType0204.U_Post = true;
            }

            FTS11STRN trnsType0205 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "10"));
            if (trnsType0205 == null)
            {
                trnsType0205 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0205.Code = "T0205";
                trnsType0205.Name = "Reclassed Out";
                trnsType0205.U_TrnsGrp = "OUT";
                trnsType0205.U_Post = true;
            }

            FTS11STRN trnsType0301 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "11"));
            if (trnsType0301 == null)
            {
                trnsType0301 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0301.Code = "T0301";
                trnsType0301.Name = "Review Useful Life";
                trnsType0301.U_TrnsGrp = "ADJ";
                trnsType0301.U_Post = false;
            }

            FTS11STRN trnsType0302 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "12"));
            if (trnsType0302 == null)
            {
                trnsType0302 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0302.Code = "T0302";
                trnsType0302.Name = "Revalue Cost";
                trnsType0302.U_TrnsGrp = "ADJ";
                trnsType0302.U_Post = true;
            }

            FTS11STRN trnsType0303 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "13"));
            if (trnsType0303 == null)
            {
                trnsType0303 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0303.Code = "T0303";
                trnsType0303.Name = "Adjust Accumulated Depr";
                trnsType0303.U_TrnsGrp = "ADJ";
                trnsType0303.U_Post = true;
            }

            FTS11STRN trnsType0304 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "14"));
            if (trnsType0304 == null)
            {
                trnsType0304 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0304.Code = "T0304";
                trnsType0304.Name = "Move";
                trnsType0304.U_TrnsGrp = "ADJ";
                trnsType0304.U_Post = true;
            }

            FTS11STRN trnsType0305 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "15"));
            if (trnsType0305 == null)
            {
                trnsType0305 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType0305.Code = "T0305";
                trnsType0305.Name = "Revalue Residual Value";
                trnsType0305.U_TrnsGrp = "ADJ";
                trnsType0305.U_Post = true;
            }

            FTS11STRN trnsType9901 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "16"));
            if (trnsType9901 == null)
            {
                trnsType9901 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType9901.Code = "T9901";
                trnsType9901.Name = "SL Depreciation";
                trnsType9901.U_TrnsGrp = "DEP";
                trnsType9901.U_Post = true;
            }

            FTS11STRN trnsType9902 = ObjectSpace.FindObject<FTS11STRN>(new BinaryOperator("DocEntry", "17"));
            if (trnsType9902 == null)
            {
                trnsType9902 = ObjectSpace.CreateObject<FTS11STRN>();
                trnsType9902.Code = "T9902";
                trnsType9902.Name = "RB Depreciation";
                trnsType9902.U_TrnsGrp = "DEP";
                trnsType9902.U_Post = true;
            }

            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<SystemUsers>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<SystemUsers>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<SystemUsers>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);                
            }
            return defaultRole;
        }
    }
}
