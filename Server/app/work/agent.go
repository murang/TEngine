package work

import (
	"sync"

	"github.com/murang/potato/net"
	"google.golang.org/protobuf/proto"
)

var sessId2Agent = sync.Map{}

type Agent struct {
	session *net.Session
	user    *User
}

func AddAgent(session *net.Session) {
	agent := &Agent{
		session: session,
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
	a.session.Send(msg)
}
