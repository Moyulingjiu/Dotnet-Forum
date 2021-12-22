<template>
	<view>
		<view class="title">
			<h1>{{ state.title }}</h1>
			<image :src="state.like?likeIcon:unlikeIcon" @click="like()"></image>
			<text>{{ state.likeNumber }}</text>
		</view>
		<view class="avater" @click="userdetail(state.userId)">
			<image :src="state.userAvater" mode="aspectFill"></image>
			<text>{{ state.userName }}</text>
		</view>
		<view class="text">
			<text selectable space="emsp">{{ state.text }}</text>
		</view>
		<view class="new_comment">
			<textarea auto-height placeholder="在此输入评论" v-model="mycomment" />
			<view @click="sendComment()"><text>发布</text></view>
		</view>
		<view class="comment">
			<view class="comment_box" v-for="(item , index) in comment">
				<view class="comment_avater" @click="userdetail(item.userId)">
					<image :src="item.userAvater" mode="aspectFill"></image>
				</view>
				<view class="comment_text_container">
					<view space="emsp" class="comment_user" @click="userdetail(item.userId)">{{ item.userName }}</view>
					<view space="emsp" class="comment_text">{{ item.text }}</view>
					<view class="comment_button">
						<text>{{ item.gmtCreate }}</text>
						<text class="comment_reply" @click="showReplyBox(index, item.id)">回复</text>
						<text class="comment_delete" @click="deleteComment(item.id)" v-if="item.userId = userId">删除</text>
					</view>
					<view class="subcomment" v-for="(subitem , subindex) in item.subcomment">
						<view class="comment_avater" @click="userdetail(subitem.userId)">
							<image :src="subitem.userAvater" mode="aspectFill"></image>
						</view>
						<view class="comment_text_container">
							<view space="emsp" class="comment_user" @click="userdetail(subitem.userId)">{{ subitem.userName }}</view>
							<view space="emsp" class="comment_text">
								<text class="reply_text" v-if="subitem.reply.length!=0" @click="userdetail(subitem.replyUserId)">{{ subitem.reply }}</text>
								<text>{{ subitem.text }}</text>
							</view>
							<view class="comment_button">
								<text>{{ subitem.gmtCreate }}</text>
								<text class="comment_reply" @click="showReplyBox(index, subitem.id)">回复</text>
								<text class="comment_delete" @click="deleteComment(subitem.id)" v-if="subitem.userId = userId">删除</text>
							</view>
						</view>
					</view>
					<view v-if="item.subcommentPage<item.subcommentTotal-1" class="comment_show_more">
						<text>点击展开更多评论</text>
					</view>
					<view class="comment_reply_box" v-if="item.needReply">
						<textarea auto-height placeholder="在此输入回复" v-model="item.replyText"></textarea>
						<view @click="sendReplyComment(index)"><text>回复</text></view>
					</view>
				</view>
			</view>
		</view>

		<view class="bottom_tips">
			<text>{{ (commentPage >= commentTotal - 1) ? bottomTipsNoMore : bottomTips }}</text>
		</view>
		
		<uni-popup ref="popup_error_empty" type="message">
			<uni-popup-message type="error" message="评论不能为空" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="评论成功" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_delete_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="2000"></uni-popup-message>
		</uni-popup>
		
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as stateApi from "../../api/state.js"
	import * as commentApi from "../../api/comment.js"
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
				userId: 0, // 用户id
				stateId: 0,
				state: {
					id: 2,
					title: '小石潭记',
					userId: 2, // 用户id
					userName: '用户2', // 用户名
					userAvater: '../../static/avater.jpg', // 头像
					text: '  从小丘西行百二十步，隔篁竹，闻水声，如鸣珮环，心乐之。伐竹取道，下见小潭，水尤清冽。全石以为底，近岸，卷石底以出，为坻，为屿，为嵁，为岩。青树翠蔓，蒙络摇缀，参差披拂。(珮 通：佩)\n  潭中鱼可百许头，皆若空游无所依。日光下澈，影布石上，佁然不动；俶尔远逝，往来翕忽。似与游者相乐。(下澈 一作：下彻)\n潭西南而望，斗折蛇行，明灭可见。其岸势犬牙差互，不可知其源。\n坐潭上，四面竹树环合，寂寥无人，凄神寒骨，悄怆幽邃。以其境过清，不可久居，乃记之而去。\n  同游者：吴武陵，龚古，余弟宗玄。隶而从者，崔氏二小生，曰恕己，曰奉壹。',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: false,
					likeNumber: 0
				},
				commentPage: 0,
				commentTotal: 1,
				comment: [{
						id: 1,
						stateId: 1,
						fatherId: 0,
						replyId: 1,
						text: '一个测试的正文',
						userId: 1,
						userName: '作者的名字',
						userAvater: '../../static/avater.jpg',
						gmtCreate: '2021年7月20日 20:00',
						subcomment: [{
								id: 2,
								stateId: 1,
								fatherId: 1,
								replyId: 1,
								text: '一个测试的回复正文',
								userId: 1,
								username: '作者的名字',
								userAvater: '../../static/avater.jpg',
								gmtCreate: '2021年7月20日 20:00',
								reply: '',
								replyUserId: 0
							}
						],
						subcommentPage: 0,
						subcommentTotal: 1,
						needReply: false, // 是否显示回复框
						replyText: '', // 想要回复的text
						replyCommentId: 1 // 想要回复的评论id
					}
				],
				mycomment: ''
			}
		},
		onLoad(options) {
			this.stateId = options.id
		},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.isRefresh = true
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		methods: {
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				} else if (this.stateId == 0 || typeof this.stateId === "undefined") {
					uni.switchTab({
						url: '../index/index'
					})
				} else {
					userApi.select().then(data => {
						if (typeof data == 'undefined') {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.userId = data.data.id
						}
					})
					stateApi.select(this.stateId).then(data => {
						if (typeof data == 'undefined') {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.state.id = data.data.id
							this.state.title = data.data.title
							this.state.text = data.data.text
							this.state.userId = data.data.userId
							this.state.userName = data.data.userName
							this.state.userAvater = data.data.userAvater
							this.state.share = data.data.shareState
							this.state.gmtCreate = data.data.gmt_create
							this.state.like = data.data.like
							this.state.likeNumber = data.data.likeNumber
						}
					})
					this.comment = []
					this.commentPage = 0
					this.commentTotal = 1
					this.loadComment()
				}
			},
			loadComment(page=this.commentPage) {
				commentApi.selectAllByStateId(this.stateId, page).then(data => {
					if (typeof data == 'undefined') {
						uni.showToast({
							title: '服务器错误',
							icon: "error",
							mask: true,
							duration: 2000
						})
					} else if (data.code != 200) {
						uni.showToast({
							title: data.msg,
							icon: "error",
							mask: true,
							duration: 2000
						})
					} else {
						this.commentTotal = data.data.total
						for (let key in data.data.items) {
							if (key != 'length') {
								let commentItem = {
									id: data.data.items[key].commentId,
									stateId: data.data.items[key].state_id,
									fatherId: data.data.items[key].father_id,
									replyId: data.data.items[key].reply_id,
									text: data.data.items[key].text,
									userId: data.data.items[key].userId,
									userName: data.data.items[key].userName,
									userAvater: data.data.items[key].userAvater,
									gmtCreate: data.data.items[key].gmt_create,
									
									subcomment: [],
									subcommentPage: 0,
									subcommentTotal: 1,
									needReply: false,
									replyText: '',
									replyCommentId: 1
								}
								commentApi.selectAllReply(commentItem.id).then(subdata => {
									if (typeof subdata == 'undefined') {
										uni.showToast({
											title: '服务器错误',
											icon: "error",
											mask: true,
											duration: 2000
										})
									} else if (subdata.code != 200) {
										uni.showToast({
											title: subdata.msg,
											icon: "error",
											mask: true,
											duration: 2000
										})
									} else {
										commentItem.subcommentTotal = subdata.data.total
										for (let key2 in subdata.data.items) {
											let subCommentItem = {
												id: subdata.data.items[key2].commentId,
												stateId: subdata.data.items[key2].state_id,
												fatherId: subdata.data.items[key2].father_id,
												replyId: subdata.data.items[key2].reply_id,
												text: subdata.data.items[key2].text,
												userId: subdata.data.items[key2].userId,
												userName: subdata.data.items[key2].userName,
												userAvater: subdata.data.items[key2].userAvater,
												gmtCreate: subdata.data.items[key2].gmt_create,
												reply: '',
												replyUserId: 0
											}
											if (subCommentItem.replyId != subCommentItem.fatherId) {
												commentApi.select(subCommentItem.id).then(data3 => {
													if (typeof data3 != 'undefined' && data3.code == 200) {
														subCommentItem.reply = '@' + data3.data.userName
														subCommentItem.replyUserId = data3.data.userId
													}
												})
											}
											commentItem.subcomment.push(subCommentItem)
										}
									}
								})
								this.comment.push(commentItem)
							}
						}
					}
				})
			},
			like() {
				let id = this.stateId
				if (this.state.like) {
					stateApi.unlike(id).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.state.like = false
							this.state.likeNumber--
							this.$forceUpdate()
						}
					})
				} else {
					stateApi.like(id).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.state.like = true
							this.state.likeNumber++
							this.$forceUpdate()
						}
					})
				}
			},
			userdetail(userId) {
				uni.navigateTo({
					url: `/pages/otherUsers/otherUsers?id=${userId}`
				})
			},
			showReplyBox(index, commentId) {
				for (let i = 0; i < this.comment.length; ++i) {
					if (i == index) {
						this.comment[i].needReply = true
						this.comment[i].replyCommentId = commentId
					} else {
						this.comment[i].needReply = false
					}
				}
			},
			deleteComment(commentId) {
				new Promise((resolve, reject) => {
					uni.showModal({
						title: '确定删除此评论吗?',
						content: ' ',
						success: function(res) {
							resolve(res)
						}
					})
				}).then(data => {
					if (data.confirm) {
						return commentApi.deleteComment(commentId)
					}
				}).then(data => {
					if (typeof data === "undefined") {
						uni.showToast({
							title: '服务器错误',
							icon: "error",
							mask: true,
							duration: 2000
						})
					} else if (data.code == 200) {
						this.$refs.popup_delete_success.open('top')
						this.refresh()
					} else {
						uni.showToast({
							title: data.msg,
							icon: "error",
							mask: true,
							duration: 2000
						})
					}
				})
			},
			sendComment() {
				if (this.mycomment.length == 0) {
					this.$refs.popup_error_empty.open('top')
				} else {
					commentApi.insert(this.stateId, 0, 0, this.mycomment).then(data => {
						if (typeof data == 'undefined') {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.$refs.popup_success.open('top')
							this.refresh()
						}
					})
				}
			},
			sendReplyComment(index) {
				if (this.comment[index].replyText.length == 0) {
					this.$refs.popup_error_empty.open('top')
				} else {
					commentApi.insert(this.stateId, this.comment[index].id, this.comment[index].replyCommentId, this.comment[index].replyText).then(data => {
						if (typeof data == 'undefined') {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							this.$refs.popup_success.open('top')
							this.refresh()
						}
					})
				}
			}
		}
	}
</script>

<style>
	.title {
		margin: 15rpx 5%;
		width: 90%;
		display: flex;
	}

	.title h1 {
		font-size: 75rpx;
	}

	.title image {
		margin-top: 15rpx;
		margin-left: 20rpx;
		width: 80rpx;
		height: 80rpx;
	}

	.title text {
		margin-top: 5rpx;
		margin-left: 10rpx;
		font-size: 75rpx;
		color: #7F7F7F;
	}

	.avater {
		margin-left: 5%;
		width: 90%;
		display: flex;
	}

	.avater image {
		border-radius: 50%;
		height: 60rpx;
		width: 60rpx;
	}

	.avater text {
		margin: 5rpx 10rpx;
		font-size: 40rpx;
		color: #808080;
	}

	.text {
		margin: 30rpx 5%;
		width: 90%;
		font-size: 40rpx;
	}

	.new_comment {
		/* margin-bottom: 30rpx; */
		margin-left: 5%;
		margin-right: 5%;
		width: 90%;
		position: relative;
		display: flex;
	}

	.new_comment textarea {
		flex: 1;
		font-size: 35rpx;
		/* margin-bottom: 30rpx; */
		padding: 10rpx;
		border: 3rpx solid #808080;
		border-radius: 10rpx;
		overflow: hidden;
	}

	.new_comment textarea:hover {
		border: 5rpx solid #00A1D6;
	}

	.new_comment view {
		text-align: center;
		margin-left: 20rpx;
		float: right;
		width: 150rpx;
		height: 65rpx;
		background-color: #00A1D6;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
		border-radius: 10rpx;
	}

	.new_comment view text {
		color: white;
		height: 100%;
		vertical-align: middle;
	}

	.comment {
		width: 90%;
		margin-left: 5%;
		padding: 10rpx;
	}

	.comment_box {
		width: 100%;
		display: flex;
		margin-top: 40rpx;
	}

	.comment_avater image {
		width: 100rpx;
		height: 100rpx;
		border-radius: 50%;
	}

	.comment_text_container {
		width: 100%;
		margin-left: 30rpx;
	}

	.comment_user {
		font-size: 30rpx;
		font-weight: bold;
		color: #6D757A;
	}

	.comment_text {
		font-size: 35rpx;
		color: #222222;
	}

	.comment_button {
		margin-top: 15rpx;
		font-size: 25rpx;
		color: #99A2AA;
	}

	.comment_reply {
		padding: 0rpx 15rpx;
		margin-left: 10rpx;
	}

	.comment_reply:hover {
		color: #00A1D6;
		border-radius: 10rpx;
		background-color: #E5E9EF;
	}
	
	.comment_delete {
		padding: 0rpx 15rpx;
	}

	.comment_delete:hover {
		color: #FF0000;
		border-radius: 10rpx;
		background-color: #E5E9EF;
	}

	.subcomment {
		width: 100%;
		margin-top: 30rpx;
		display: flex;
	}
	
	.comment_show_more text {
		padding: 0 10rpx;
		font-size: 30rpx;
		color: #00A1D6;
	}
	
	.comment_show_more text:hover {
		border-radius: 10rpx;
		background-color: #E5E9EF;
	}

	.comment_reply_box {
		margin-top: 15rpx;
		width: 100%;
		position: relative;
		display: flex;
	}

	.comment_reply_box textarea {
		flex: 1;
		width: 100%;
		font-size: 35rpx;
		padding: 10rpx;
		border: 3rpx solid #808080;
		border-radius: 10rpx;
	}

	.comment_reply_box textarea:hover {
		border: 5rpx solid #00A1D6;
	}

	.comment_reply_box view {
		text-align: center;
		margin-left: 20rpx;
		float: right;
		width: 150rpx;
		height: 65rpx;
		background-color: #00A1D6;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
		border-radius: 10rpx;
	}

	.comment_reply_box view text {
		color: white;
		height: 100%;
		vertical-align: middle;
	}

	.bottom_tips {
		margin-top: 30rpx;
		text-align: center;
		height: 200rpx;
	}

	.bottom_tips text {
		color: #A8A8A8;
		bottom: 0;
	}
	
	.reply_text {
		color: #00A1D6;
		margin-right: 10rpx;
	}
</style>
