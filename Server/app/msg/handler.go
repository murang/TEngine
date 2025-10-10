package msg

import (
	"github.com/murang/potato/log"
	"github.com/murang/potato/net"
)

type Handler struct {
}

func (h *Handler) IsMsgInRoutine() bool {
	return true
}

func (h *Handler) OnSessionOpen(session *net.Session) {
	log.Sugar.Infof("session open: %d", session.ID())
	AddAgent(session)
}

func (h *Handler) OnSessionClose(session *net.Session) {
	log.Sugar.Infof("session close: %d", session.ID())
	RemoveAgent(session)
}

func (h *Handler) OnMsg(session *net.Session, msg any) {
	log.Sugar.Infof("session: %d, msg: %v", session.ID(), msg)
	agent := GetAgentBySessionId(session.ID())
	if agent == nil {
		log.Sugar.Errorf("session: %d, agent not found", session.ID())
		return
	}

}
