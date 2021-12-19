<template>
	<view>
		<view class="title">
			<h1>{{ state.title }}</h1>
			<image :src="state.like?likeIcon:unlikeIcon" @click="like()"></image>
		</view>
		<view class="avater">
			<image :src="state.userAvater" mode="aspectFill"></image>
			<text>{{ state.userName }}</text>
		</view>
		<view class="text">
			<text selectable space="emsp">{{ state.text }}</text>
		</view>
		<view class="new_comment">
			<editor type="text" placeholder="在此输入评论" v-model="comment" />
			<button>发布</button>
		</view>
		<view class="comment">
			
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
				comment: '', // 用户想要发布的评论
				state: {
					id: 2,
					title: '小石潭记',
					userId: 2, // 用户id
					userName: '用户2', // 用户名
					userAvater: '../../static/avater.jpg', // 头像
					text: '  从小丘西行百二十步，隔篁竹，闻水声，如鸣珮环，心乐之。伐竹取道，下见小潭，水尤清冽。全石以为底，近岸，卷石底以出，为坻，为屿，为嵁，为岩。青树翠蔓，蒙络摇缀，参差披拂。(珮 通：佩)\n  潭中鱼可百许头，皆若空游无所依。日光下澈，影布石上，佁然不动；俶尔远逝，往来翕忽。似与游者相乐。(下澈 一作：下彻)\n潭西南而望，斗折蛇行，明灭可见。其岸势犬牙差互，不可知其源。\n坐潭上，四面竹树环合，寂寥无人，凄神寒骨，悄怆幽邃。以其境过清，不可久居，乃记之而去。\n  同游者：吴武陵，龚古，余弟宗玄。隶而从者，崔氏二小生，曰恕己，曰奉壹。',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: false
				},
				comments: [
					{
						id: 1,
						State_id: 1,
						Father_id: 0,
						Reply_id: 0,
						Text: '测试评论',
						userId: 1,
						username: '测试用户名',
						userAvater: '../../static/avater.jpg', // 头像
						sonComments: [
							{
								id: 2,
								State_id: 1,
								Father_id: 1,
								Reply_id: 0,
								Text: '测试评论回复',
								userId: 1,
								username: '测试用户名',
								userAvater: '../../static/avater.jpg', // 头像
							}
						]
					},
					{
						id: 3,
						State_id: 1,
						Father_id: 0,
						Reply_id: 0,
						Text: '测试评论',
						userId: 1,
						username: '测试用户名',
						userAvater: '../../static/avater.jpg', // 头像
						sonComments: [
						]
					}
				]
			}
		},
		methods: {
			submitComment() {
				console.log('评论')
			},
			like() {
				this.state.like = !this.state.like
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
		margin-top: 20rpx;
		margin-left: 20rpx;
		width: 70rpx;
		height: 70rpx;
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
		margin-bottom: 30rpx;
		margin-left: 5%;
		width: 90%;
	}
	
	.new_comment editor {
		margin-bottom: 30rpx;
		padding: 10rpx;
		height: 100rpx;
		border: 3rpx solid #808080;
		border-radius: 10rpx;
	}
	
	.new_comment button {
		border-radius: 40rpx;
		height: 80rpx;
		background-image: linear-gradient(to top, #48c6ef 0%, #6f86d6 100%);
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}
</style>
