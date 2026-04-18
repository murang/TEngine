package work

import (
	"sync"

	"github.com/murang/potato/log"
	"github.com/murang/potato/net"
	"google.golang.org/protobuf/proto"
)

var sessId2Agent = sync.Map{}

type Agent struct {
	Session *net.Session
	User    *User
}

func AddAgent(session *net.Session) {
	agent := &Agent{
		Session: session,
	}
	sessId2Agent.Store(session.ID(), agent)
}

func RemoveAgent(session *net.Session) {
	sessId2Agent.Delete(session.ID())
}

func GetAgentBySessionId(sessionId uint64) *Agent {
	value, _ := sessId2Agent.Load(sessionId)
	return value.(*Agent)
}

func (a *Agent) SendMsg(msg proto.Message) {
	log.Sugar.Infof("user: %d, <=====: %T %v", a.User.Model.ID, msg, msg)
	a.Session.Send(msg)
}
