import { Comment } from "./comment.model";

export class CommentQuote {
	constructor(commentId?: string, quotedCommentId?: string, quotedComment?: any) {
		this.id = quotedComment?.id ?? null;
		this.commentId = quotedComment?.commentId ?? commentId;
		this.quotedCommentId = quotedComment?.quotedCommentId ?? quotedCommentId;
		this.comment = quotedComment?.comment ?? null;
		this.quotedComment = quotedComment?.quotedComment ?? null;
	}

	public id?: string;
	public commentId: string;
	public quotedCommentId: string;
	public comment?: Comment;
	public quotedComment?: Comment;
}