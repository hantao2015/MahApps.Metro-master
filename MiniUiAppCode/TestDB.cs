using System;
using System.Collections;

namespace MiniUiAppCode
{

    public class TestDB
    {
        public void UpdateTreeNode(Hashtable n) 
        {
            string sql = @"
                update plus_file 
                set 
	                name = @name,
                    type = @type,
                    size = @size,
                    url = @url,    
                    pid = @pid,
                    createdate = @createdate,
                    updatedate = @updatedate,
                    folder = @folder,
                    num = @num
                where id = @id";

            DBUtil.Execute(sql, n);
        }

        public string InsertNode(Hashtable n) 
        {
            string id = (n["id"] == null || n["id"].ToString() == "") ? Guid.NewGuid().ToString() : n["id"].ToString();
            n["id"] = id;
            string sql = "insert into plus_file (id, name, type, size, url, pid, createdate, updatedate, folder, num)"
                + " values(@id, @name, @type, @size, @url, @pid, @createdate, @updatedate, @folder, @num)";
            DBUtil.Execute(sql, n);
            return id;
        }

        public void RemoveNode(Hashtable n) 
        {
            DBUtil.Execute("delete from plus_file where id = @id", n);
        }

        public ArrayList GetDepartments()
        {
            string sql = @"
select *
from t_department";
            ArrayList data = DBUtil.Select(sql);
            return data;
        }
        public Hashtable GetDepartment(String id)
        {
            string sql = "select * from t_department where id = '" + id + "'";
            ArrayList data = DBUtil.Select(sql);
            return data.Count > 0 ? (Hashtable)data[0] : null;
        }
        public ArrayList GetPositions()
        {
            string sql = "select * from t_position";
            ArrayList data = DBUtil.Select(sql);
            return data;
        }
        public ArrayList GetEducationals()
        {
            string sql = "select * from t_educational";
            ArrayList data = DBUtil.Select(sql);
            return data;
        }
        public ArrayList GetPositionsByDepartmenId(string departmentId)
        {

            string sql = "select * from t_position where dept_id = '" + departmentId + "'";
            ArrayList dataAll = DBUtil.Select(sql);

            return dataAll;

        }


        public Hashtable GetDepartmentEmployees(string departmentId, int index, int size)
        {
            string sql = "select * from t_employee where dept_id = '" + departmentId + "'";
            ArrayList dataAll = DBUtil.Select(sql);

            //实现一个内存分页(实际应该使用SQL分页)
            ArrayList data = new ArrayList();
            int start = index * size, end = start + size;

            for (int i = 0, l = dataAll.Count; i < l; i++)
            {
                Hashtable record = (Hashtable)dataAll[i];
                if (record == null) continue;
                if (start <= i && i < end)
                {
                    data.Add(record);
                }
            }

            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = dataAll.Count;

            return result;
        }

        public Hashtable SearchEmployees(string key, int index, int size, string sortField, string sortOrder)
        {
            //System.Threading.Thread.Sleep(300);

            string sql = @"
select a.*, b.name dept_name, c.name position_name, d.name educational_name
from t_employee a
left join t_department b
on a.dept_id = b.id 
left join t_position c
on a.position = c.id 
left join t_educational d
on a.educational = d.id 
where a.name like '%" + key + "%'";           

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }
            else
            {
                sql += " order by createtime desc";
            }

            ArrayList dataAll = DBUtil.Select(sql);

            //System.Threading.Thread.Sleep(10000);       //延时10秒，模拟读取数据耗费时间

            //实现一个内存分页(实际应该使用SQL分页)
            ArrayList data = new ArrayList();
            int start = index * size, end = start + size;

            for (int i = 0, l = dataAll.Count; i < l; i++)
            {
                Hashtable record = (Hashtable)dataAll[i];
                if (record == null) continue;
                if (start <= i && i < end)
                {
                    data.Add(record);
                }
            }

            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = dataAll.Count;

            //生成一些汇总信息
            //1)年龄：minAge, maxAge, avgAge
            ArrayList ages = DBUtil.Select("select min(age) as minAge, max(age) as maxAge, avg(age) as avgAge from t_employee");
            Hashtable ageInfo = ages[0] as Hashtable;
            result["minAge"] = ageInfo["minAge"];
            result["maxAge"] = ageInfo["maxAge"];
            result["avgAge"] = ageInfo["avgAge"];

            //2)总员工数 total



            return result;
        }

        public Hashtable SearchEmployees2(string key, int index, int size, string sortField, string sortOrder)
        {
            //System.Threading.Thread.Sleep(300);

            string sql = @"
select a.*, b.name dept_name, c.name position_name, d.name educational_name
from t_employee a
left join t_department b
on a.dept_id = b.id 
left join t_position c
on a.position = c.id 
left join t_educational d
on a.educational = d.id 
where a.name like '%" + key + "%'";

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }
            else
            {
                sql += " order by createtime desc";
            }

            ArrayList dataAll = DBUtil.Select(sql);

            //System.Threading.Thread.Sleep(1000);       //延时1秒，模拟读取数据耗费时间

            //实现一个内存分页(实际应该使用SQL分页)
            ArrayList data = new ArrayList();
            int start = index * size, end = start + size;

            for (int i = 0, l = dataAll.Count; i < l; i++)
            {
                Hashtable record = (Hashtable)dataAll[i];
                if (record == null) continue;
                if (start <= i && i < end)
                {
                    data.Add(record);
                }
            }

            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = dataAll.Count;

            //生成一些汇总信息
            //1)年龄：minAge, maxAge, avgAge
            ArrayList ages = DBUtil.Select("select min(age) as minAge, max(age) as maxAge, avg(age) as avgAge from t_employee");
            Hashtable ageInfo = ages[0] as Hashtable;
            result["minAge"] = ageInfo["minAge"];
            result["maxAge"] = ageInfo["maxAge"];
            result["avgAge"] = ageInfo["avgAge"];

            //2)总员工数 total



            return result;
        }



        public Hashtable GetEmployee(string id)
        {
            string sql = "select * from t_employee where id = '" + id + "'";
            ArrayList data = DBUtil.Select(sql);
            return data.Count > 0 ? (Hashtable)data[0] : null;
        }
        public string InsertEmployee(Hashtable user)
        {
            string id = (user["id"] == null || user["id"].ToString() == "") ? Guid.NewGuid().ToString() : user["id"].ToString();
            user["id"] = id;

            if (user["name"] == null) user["name"] = "";
            if (user["gender"] == null || String.IsNullOrEmpty(user["gender"].ToString())) user["gender"] = 0;

            string sql = "insert into t_employee (id, loginname, name, age, married, gender, birthday, country, city, dept_id, position, createtime, salary, educational, school, email, remarks)"
                + " values(@id, @loginname, @name, @age, @married, @gender, @birthday, @country, @city, @dept_id, @position, @createtime, @salary, @educational, @school, @email, @remarks)";
            DBUtil.Execute(sql, user);
            return id;
        }
        public void DeleteEmployee(string userId)
        {
            Hashtable user = new Hashtable();
            user["id"] = userId;
            DBUtil.Execute("delete from t_employee where id = @id", user);
        }
        public void UpdateEmployee(Hashtable user)
        {
            Hashtable db_user = GetEmployee(user["id"].ToString());
            foreach (DictionaryEntry de in user)
            {
                db_user[de.Key] = de.Value;
            }

            DeleteEmployee(user["id"].ToString());
            InsertEmployee(db_user);
        }

        public void UpdateDepartment(Hashtable d)
        {
            Hashtable db_d = GetDepartment(d["id"].ToString());
            foreach (DictionaryEntry de in d)
            {
                db_d[de.Key] = de.Value;
            }
            string sql = @"
update t_department 
set 
	name = @name,
	manager = @manager,
    manager_name = @manager_name
where id = @id";

            DBUtil.Execute(sql, db_d);
        }
    }
}