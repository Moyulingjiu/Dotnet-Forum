<template>
	<view>
		<view class="title">
			<h1>{{ state.title }}</h1>
			<image :src="state.like?likeIcon:unlikeIcon" @click="like()"></image>
			<text>{{ state.likeNumber }}</text>
		</view>
		<view class="avater">
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
				<view class="comment_avater">
					<image src="../../static/avater.jpg" mode="aspectFill"></image>
				</view>
				<view class="comment_text_container">
					<view space="emsp" class="comment_user">{{ item.username }}</view>
					<view space="emsp" class="comment_text">{{ item.text }}</view>
					<view class="comment_button">
						<text>{{ item.gmtCreate }}</text>
						<text class="comment_reply" @click="showReplyBox(index, item.id)">回复</text>
					</view>
					<view class="subcomment" v-for="(subitem , subindex) in item.subcomment">
						<view class="comment_avater">
							<image src="../../static/avater.jpg" mode="aspectFill"></image>
						</view>
						<view class="comment_text_container">
							<view space="emsp" class="comment_user">{{ subitem.username }}</view>
							<view space="emsp" class="comment_text">{{ subitem.text }}</view>
							<view class="comment_button">
								<text>{{ subitem.gmtCreate }}</text>
								<text class="comment_reply" @click="showReplyBox(index, subitem.id)">回复</text>
							</view>
						</view>
					</view>
					<view class="comment_show_more">
						<text>点击展开更多评论</text>
					</view>
					<view class="comment_reply_box" v-if="item.needReply">
						<textarea auto-height placeholder="在此输入回复"></textarea>
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
	</view>
</template>

<script>
	import * as stateApi from "../../api/state.js"
	import * as commentApi from "../../api/comment.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
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
						username: '作者的名字',
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
								gmtCreate: '2021年7月20日 20:00'
							},
							{
								id: 3,
								stateId: 1,
								fatherId: 1,
								replyId: 1,
								text: '一个测试的回复正文2',
								userId: 1,
								username: '作者的名字2',
								userAvater: '../../static/avater.jpg',
								gmtCreate: '2021年7月20日 20:00'
							}
						],
						subcommentPage: 0,
						subcommentTotal: 1,
						needReply: false, // 是否显示回复框
						replyText: '', // 想要回复的text
						replyCommentId: 1 // 想要回复的评论id
					},
					{
						id: 1,
						stateId: 2,
						fatherId: 0,
						replyId: 1,
						text: '一个测试正文！你好世界',
						userId: 1,
						username: '作者的名字',
						userAvater: '../../static/avater.jpg',
						gmtCreate: '2021年7月20日 20:00',
						subcomment: [],
						subcommentPage: 0,
						subcommentTotal: 1,
						needReply: false, // 是否显示回复框
						replyText: '', // 想要回复的text
						replyCommentId: 1 // 想要回复的评论id
					},
					{
						id: 1,
						stateId: 2,
						fatherId: 0,
						replyId: 1,
						text: '一个测试的正文',
						userId: 1,
						username: '作者的名字',
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
							gmtCreate: '2021年7月20日 20:00'
						}],
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
							this.state.userId = data.data.user_id
							this.state.userName = data.data.user_name
							this.state.userAvater = data.data.user_avater
							this.state.share = data.data.share_state
							this.state.gmtCreate = data.data.gmt_create
							this.state.like = data.data.like
							this.state.likeNumber = data.data.likeNumber
						}
					})
				}
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
						}
					})
				}
			},
			sendReplyComment(index) {
				if (this.comment[index].replyText.length == 0) {
					this.$refs.popup_error_empty.open('top')
				} else {
					
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
		font-size: 30rpx;
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
</style>
