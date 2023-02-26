using Breezee.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Breezee.Core.Entity
{
	[Serializable]
	public partial class DT_SYS_MENU_BUTTON : DbEntity
	{

		//�Զ����ɱ���
		public static string TName = "SYS_MENU_BUTTON"; ////����ʵ��
		public static DbEntity NewEntity(){ return new DT_SYS_MENU_BUTTON();} ////����ʵ��
		 public override DbObjectType EntType {  get {return DbObjectType.Table;} }
		 public override string DBTableName {  get {return "SYS_MENU_BUTTON";} }
		//�Զ������ֶ�
		 public static string BUTTON_ID = "BUTTON_ID";
		 public DbField EF_BUTTON_ID = new DbField().DBColName("BUTTON_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string CREATE_TIME = "CREATE_TIME";
		 public DbField EF_CREATE_TIME = new DbField().DBColName("CREATE_TIME").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string CREATOR = "CREATOR";
		 public DbField EF_CREATOR = new DbField().DBColName("CREATOR").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string CREATOR_ID = "CREATOR_ID";
		 public DbField EF_CREATOR_ID = new DbField().DBColName("CREATOR_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string FORM_ID = "FORM_ID";
		 public DbField EF_FORM_ID = new DbField().DBColName("FORM_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string IS_ENABLED = "IS_ENABLED";
		 public DbField EF_IS_ENABLED = new DbField().DBColName("IS_ENABLED").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string IS_GIVE_RIGHT = "IS_GIVE_RIGHT";
		 public DbField EF_IS_GIVE_RIGHT = new DbField().DBColName("IS_GIVE_RIGHT").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string IS_SYSTEM = "IS_SYSTEM";
		 public DbField EF_IS_SYSTEM = new DbField().DBColName("IS_SYSTEM").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string LAST_UPDATED_TIME = "LAST_UPDATED_TIME";
		 public DbField EF_LAST_UPDATED_TIME = new DbField().DBColName("LAST_UPDATED_TIME").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string MENU_BUTTON_ID = "MENU_BUTTON_ID";
		 public DbField EF_MENU_BUTTON_ID = new DbField().DBColName("MENU_BUTTON_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string MENU_ID = "MENU_ID";
		 public DbField EF_MENU_ID = new DbField().DBColName("MENU_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string MODIFIER = "MODIFIER";
		 public DbField EF_MODIFIER = new DbField().DBColName("MODIFIER").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string MODIFIER_ID = "MODIFIER_ID";
		 public DbField EF_MODIFIER_ID = new DbField().DBColName("MODIFIER_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string ORG_ID = "ORG_ID";
		 public DbField EF_ORG_ID = new DbField().DBColName("ORG_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string REMARK = "REMARK";
		 public DbField EF_REMARK = new DbField().DBColName("REMARK").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string SORT_ID = "SORT_ID";
		 public DbField EF_SORT_ID = new DbField().DBColName("SORT_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string SYS_ID = "SYS_ID";
		 public DbField EF_SYS_ID = new DbField().DBColName("SYS_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string TFLAG = "TFLAG";
		 public DbField EF_TFLAG = new DbField().DBColName("TFLAG").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string UPDATE_CONTROL_ID = "UPDATE_CONTROL_ID";
		 public DbField EF_UPDATE_CONTROL_ID = new DbField().DBColName("UPDATE_CONTROL_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public override List<DbField> DbColumnList
		 {
			get
			{
				 var DbColumn = new List<DbField>(); 
				 DbColumn.AddRange(new DbField[] { EF_BUTTON_ID,EF_CREATE_TIME,EF_CREATOR,EF_CREATOR_ID,EF_FORM_ID,EF_IS_ENABLED,EF_IS_GIVE_RIGHT,EF_IS_SYSTEM,EF_LAST_UPDATED_TIME,EF_MENU_BUTTON_ID,EF_MENU_ID,EF_MODIFIER,EF_MODIFIER_ID,EF_ORG_ID,EF_REMARK,EF_SORT_ID,EF_SYS_ID,EF_TFLAG,EF_UPDATE_CONTROL_ID });
				 return DbColumn;
			}
		 }
		 public override List<string> ColumnStringList
		 {
			get
			{
				 var DbColumn = new List<string>();
				 DbColumn.AddRange(new string[] { "BUTTON_ID","CREATE_TIME","CREATOR","CREATOR_ID","FORM_ID","IS_ENABLED","IS_GIVE_RIGHT","IS_SYSTEM","LAST_UPDATED_TIME","MENU_BUTTON_ID","MENU_ID","MODIFIER","MODIFIER_ID","ORG_ID","REMARK","SORT_ID","SYS_ID","TFLAG","UPDATE_CONTROL_ID" });
				 return DbColumn;
			}
		 }
	}
}
