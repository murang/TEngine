package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"server/cfg"
)

func loader(file string) ([]map[string]interface{}, error) {
	if bytes, err := ioutil.ReadFile("./configs/" + file + ".json"); err != nil {
		return nil, err
	} else {
		jsonData := make([]map[string]interface{}, 0)
		if err = json.Unmarshal(bytes, &jsonData); err != nil {
			return nil, err
		}
		return jsonData, nil
	}
}

func main() {
	if tables, err := cfg.NewTables(loader); err != nil {
		println(err.Error())
	} else {
		fmt.Printf("%v", tables.TbItem.GetDataList()[0])
	}

}
