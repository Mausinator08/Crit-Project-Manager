import { Comment } from "./comment.model";

export class MentionedUserComment {
	constructor(applicationUserId: string, commentId: string, mentionedUserComment?: any) {
		this.id = mentionedUserComment?.id ?? null;
		this.applicationUserId = mentionedUserComment?.applicationUserId ?? applicationUserId;
		this.commentId = mentionedUserComment?.commentId ?? commentId;
		this.comment = mentionedUserComment?.comment ?? null;
	}

	public id?: string;
	public applicationUserId: string;
	public commentId: string;
	public comment?: Comment;
}