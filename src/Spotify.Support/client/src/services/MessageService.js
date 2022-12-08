import $api from "../utils/api";

export default class MessageService {
   static async getMessageHistory(offset, limit) {
       return $api.get("api/message/history", {params: {offset, limit}})
   }
}