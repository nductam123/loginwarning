create database monitor_log;
use monitor_log;

create table users(
	user_id varchar(6) primary key,
	roles varchar(15),
	uname varchar(30),
	pwd varchar(8)
);
insert into users(user_id,roles,uname,pwd) values ('1' , '0','admin','123456');

create table history(
	log_id varchar(6) primary key,
	user_id varchar(6) foreign key references users(user_id),
	urls varchar(MAX),
	title nvarchar(MAX),
	violation bit,
	session_time float,
	created_at datetime,
);

create table violations(
	ticket_id varchar(6),
	log_id varchar(6) foreign key references history(log_id),
	session_time float,
	created_at datetime
);
