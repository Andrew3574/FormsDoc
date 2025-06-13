create type role as enum ('user', 'admin');
create type state as enum ('active', 'blocked');
create type accessibility as enum ('public', 'restricted');

create table users(
	id serial primary key,
	email varchar(32) not null,
	role role default 'user',
	state state default 'active',
	name varchar(20) not null,
	surname varchar(20) not null,
	passwordhash varchar(32) not null,
	image_url varchar(32)
);
create table Topics(
	id serial primary key,
	name varchar(32) not null
);

create table Tags(
	id serial primary key,
	name varchar(32) not null
);

create table Forms(
	id serial primary key,
	user_id int references users(id) on delete cascade,
	created_at timestamptz(0) default now(),
	title varchar(32) not null,
	description varchar(100),
	topic_id int references topics(id) on delete cascade,
	accessibility accessibility default 'public',
	image_url varchar(32),
	version int default 1
);

create table AccessForm_Users(
	id serial primary key,
	form_id int references forms(id) on delete cascade,
	user_id int references users(id) on delete cascade,
	unique (form_id, user_id)
);

create table Likes(
	id serial primary key,
	user_id int references Users(id) on delete cascade,
	form_id int references Forms(id) on delete cascade,
	unique (user_id, form_id)
);

create table Comments(
	id serial primary key,
	user_id int references Users(id) on delete cascade,
	form_id int references Forms(id) on delete cascade,
	created_at timestamptz(0) default now()
);

create table Form_Tags(
	id serial primary key,
	form_id int references Forms(id) on delete cascade,
	tag_id int references Tags(id) on delete cascade
);

create table Question_Types(
	id serial primary key,
	name varchar(20) not null	
);

create table Form_Questions(
	id serial primary key,
	form_id int references forms(id) on delete cascade,
	question_type_id int references Question_Types(id) on delete cascade,
	question text not null,
	description text,
	display_state boolean,
	position int not null
);

--------------------------------------
create table Form_Question_Options(
	id serial primary key,
	Form_Question_id int references Form_Questions(id) on delete cascade,
	option_value text not null,
	position int not null
);
--------------------------------------

create table Form_Answers(
	id serial primary key,
	user_id int references users(id) on delete cascade,
	form_id int references forms(id) on delete cascade,
	asnwered_at timestamptz(0) default now()
);

create table Short_Text_Answers(
	id serial primary key,
	answer_id int references form_answers(id) on delete cascade,
	form_Question_id int references Form_Questions(id) on delete cascade,
	answer varchar(50) not null
);

create table Long_Text_Answers(
	id serial primary key,
	answer_id int references form_answers(id) on delete cascade,
	form_Question_id int references Form_Questions(id) on delete cascade,
	answer varchar(200) not null
);

create table Integer_Answers(
	id serial primary key,
	answer_id int references form_answers(id) on delete cascade,
	form_Question_id int references Form_Questions(id) on delete cascade,
	answer int not null
);

create table Checkbox_Answers(
	id serial primary key,
	answer_id int references form_answers(id) on delete cascade,
	form_Question_id int references Form_Questions(id) on delete cascade,
	answer boolean default false
);

insert into topics(name) values('Education'),
('Quiz'),('Other');

insert into tags(name) values('Personality'),
('Intelligence'),('Professional skills');

insert into Question_Types(name) values('text'),
('textarea'),('uinteger'),('checkbox');

--create unique index on users(email);