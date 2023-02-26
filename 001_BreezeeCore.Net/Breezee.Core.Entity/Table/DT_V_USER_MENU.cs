using Breezee.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Breezee.Core.Entity
{
	[Serializable]
	public partial class DT_V_USER_MENU : DbEntity
	{

		//�Զ����ɱ���
		public static string TName = "V_USER_MENU"; ////����ʵ��
		public static DbEntity NewEntity(){ return new DT_V_USER_MENU();} ////����ʵ��
		 public override DbObjectType EntType {  get {return DbObjectType.Table;} }
		 public override string DBTableName {  get {return "V_USER_MENU";} }
		//�Զ������ֶ�
		 public static string MENU_ID = "MENU_ID";
		 public DbField EF_MENU_ID = new DbField().DBColName("MENU_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string SYS_ID = "SYS_ID";
		 public DbField EF_SYS_ID = new DbField().DBColName("SYS_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public static string USER_ID = "USER_ID";
		 public DbField EF_USER_ID = new DbField().DBColName("USER_ID").DBColType("VARCHAR").DBTypeSize("VARCHAR(36)").DBScale(-1).Nullable(true).PK(false).DBLen(36).FieldName("").FieldType(EntityFieldType.DbColumn).UpdateControl(false);

		 public override List<DbField> DbColumnList
		 {
			get
			{
				 var DbColumn = new List<DbField>(); 
				 DbColumn.AddRange(new DbField[] { EF_MENU_ID,EF_SYS_ID,EF_USER_ID });
				 return DbColumn;
			}
		 }
		 public override List<string> ColumnStringList
		 {
			get
			{
				 var DbColumn = new List<string>();
				 DbColumn.AddRange(new string[] { "MENU_ID","SYS_ID","USER_ID" });
				 return DbColumn;
			}
		 }
	}
}
