package main

import (
	"github.com/murang/potato/log"
	"github.com/murang/potato/net"
)

type Handler struct {
}

func (h *Handler) IsMsgInRoutine() bool {
	return false
}

func (h *Handler) OnSessionOpen(session *net.Session) {
	log.Sugar.Infof("session open: %d", session.ID())
}

func (h *Handler) OnSessionClose(session *net.Session) {
	log.Sugar.Infof("session close: %d", session.ID())
}

func (h *Handler) OnMsg(session *net.Session, msg any) {
	log.Sugar.Infof("session: %d, msg: %v", session.ID(), msg)
	session.Send(msg)
}
