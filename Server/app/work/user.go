package work

import (
	"server/db"
	"server/pb/pb"
)

type User struct {
	Model *db.User
}

func GetUserByModel(model *db.User) *User {
	user := &User{}

	user.Model = model

	return user
}

func (u *User) GetUserData() *pb.UserData {
	return &pb.UserData{
		UserInfo: u.GetUserInfo(),
	}
}

func (u *User) GetUserInfo() *pb.UserInfo {
	return &pb.UserInfo{
		Id:       uint32(u.Model.ID),
		Nickname: u.Model.GetNickname(),
	}
}
