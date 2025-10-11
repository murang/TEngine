package work

import "server/pb/pb"

func Heartbeat(agent *Agent, msg *pb.C2S_Heartbeat) {
	agent.SendMsg(&pb.S2C_Heartbeat{})
}

func Login(agent *Agent, msg *pb.C2S_Login) {

}
