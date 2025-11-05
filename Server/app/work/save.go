package work

import "server/db"

func (a *Agent) Save() {
	if len(a.User.Model.Marks) > 0 {
		updates := make([]string, 0, len(a.User.Model.Marks))
		for mark := range a.User.Model.Marks {
			updates = append(updates, mark)
		}
		db.Client.Model(a.User.Model).Select(updates).Updates(a.User.Model)
		a.User.Model.Marks = map[string]struct{}{}
	}
}

func (a *Agent) MarkUser(marks ...string) {
	for _, mark := range marks {
		a.User.Model.Marks[mark] = struct{}{}
	}
}
