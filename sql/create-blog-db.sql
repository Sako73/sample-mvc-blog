/*
*   This script creates the database for the Cooper Vision Blog Sample.
*
*   - There are two tables: 
*       - BlogPost: holds the blog posts.
*       - BlogComment: holds the comments for each BlogPost
*/

create table BlogPost (
    id      int             not null,
    title   nvarchar(256)   not null,
    post    nvarchar(4000)  not null,
    created datetime        not null, -- constraint d_BlogPost_created default getdate(),

    constraint pk_BlogPost primary key (id),
    constraint u_BlogPost_title unique (title)
);

create table BlogComment (
    id      int             not null,
    postId  int             not null,
    comment nvarchar(1024)  not null,
    created datetime        not null, -- constraint d_BlogComment_created default getdate(),

    constraint pk_BlogComment primary key (id),
    constraint fk_BlogComment2BlogPost foreign key (postId)
        references BlogPost (id)
);