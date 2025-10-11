package msg

import (
	"server/app/work"

	"github.com/murang/potato/log"
	"github.com/murang/potato/net"
	"github.com/murang/potato/util"
	"google.golang.org/protobuf/proto"
)

type Handler struct {
}

func (h *Handler) IsMsgInRoutine() bool {
	return true
}

func (h *Handler) OnSessionOpen(session *net.Session) {
	log.Sugar.Infof("session open: %d", session.ID())
	work.AddAgent(session)
}

func (h *Handler) OnSessionClose(session *net.Session) {
	log.Sugar.Infof("session close: %d", session.ID())
	work.RemoveAgent(session)
}

func (h *Handler) OnMsg(session *net.Session, msg any) {
	log.Sugar.Infof("session: %d, msg: %v", session.ID(), msg)
	agent := work.GetAgentBySessionId(session.ID())
	if agent == nil {
		log.Sugar.Errorf("session: %d, agent not found", session.ID())
		return
	}
	pid := util.TypePtrOf(msg)
	handler, ok := msgDispatcher[pid]
	if ok {
		handler(agent, msg.(proto.Message))
	}
}
