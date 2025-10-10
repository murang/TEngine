package msg

import (
	"server/app/work"
	"sync"

	"github.com/murang/potato/net"
)

var sessId2Agent = sync.Map{}

type Agent struct {
	session *net.Session
	user    *work.User
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
