<template>
	<view>
		<view v-if="tabIndex!=2" class="search_box">
			<image class="search_icon" src="../../static/search.png"></image>
			<input class="search_area" type="text" v-model="searchCondition" confirm-type="search" @confirm="search()"
				placeholder="搜索用户状态等" />
			<image class="search_cancel" v-if="searchCondition.length!=0" src="../../static/cancel.png"
				@click="clearCondition()"></image>
		</view>
		
		<view class="tabs">
		    <scroll-view scroll-x class="scroll-h" >
		    <block v-for="(tab,index) in tabBars" :key="tab.id">
		    <view class="uni-tab-item" :class="{'uni-tab-item-title-active' :tabIndex==index}" @tap="tabtap(index)">
		    <!-- 1 {{tab.name}} -->
				<text class="uni-tab-item-title" :class="tabIndex==index ? 'uni-tab-item-title-active' : ''">{{tab.name}}</text>
		    </view>
		    </block>            
		    </scroll-view>
		</view>
		
		<view v-if="tabIndex==0"><!-- 综合界面 -->
			<uni-list>
					<!-- title、note设置标题和小文字 -->
					<uni-list-item irection="row" v-for="(item,index) in comprehensiveInfo" :key="index" :title="item.title":note="item.description"
				@click="clickMessage(item.id)" link>
						<template v-slot:header>
							<image class="image-1" :src="item.coverUrl" mode="aspectFill"></image>
						</template>
					</uni-list-item>
			</uni-list>
		</view>
		
		<view v-if="tabIndex==1"><!-- 用户界面 -->
			<uni-list>
					<!-- title、note设置标题和小文字 -->
					<uni-list-item irection="row" v-for="(item,index) in userInfo" :key="index" :title="item.username":note="item.description"
				@click="clickUser(item.id)" link>
						<template v-slot:header>
							<image class="image-1" :src="item.imgUrl" mode="aspectFill"></image>
						</template>
					</uni-list-item>
			</uni-list>
		</view>
		
		<view v-if="tabIndex==2">
			<view>当前输入：

				<view style="text-align: center;">姓名:{{searchName}}</view>
				<view style="text-align: center;">学号:{{searchNum}}</view>
				<view style="text-align: center;">学院:{{searchCollege}}</view>
				<view style="text-align: center;">系别:{{searchDepartment}}</view>
				<view style="text-align: center;">专业:{{searchSpeciality}}</view>
				
			</view>
			<picker @change="bindPickerChange" :range="array">	<!-- //@change用于获取下拉框改变的值，:range用于循环遍历array数组将数组内容循环 -->
				<label>点击选择输入类型：</label>
				<label class="mid">{{array[index]}}</label>		<!-- //循环array数组index为索引 -->
			</picker>

			<view class="search_box">
				<input class="search_area" type="text" v-model="searchCondition" confirm-type="search" @confirm="search()"
					placeholder="搜索用户状态等" />
				<image class="search_cancel" v-if="searchCondition.length!=0" src="../../static/cancel.png"
					@click="clearCondition()"></image>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {			
				
				array:['---请选择---','姓名','学号','学院','系','专业'],
				index:0,
				
				searchName:"asd",
				searchNum:"asd",
				searchCollege:"asd",
				searchDepartment:"asd",
				searchSpeciality:"asasdd",
				
				searchCondition: '', // 搜索条件
				tabIndex:0,
				tabBars:[
				    { name:"综合",id:"application"},
				    { name:"用户",id:"user"},
					{name:"test",id:"111"}
				],
				
				comprehensiveInfo:[
					{
						id:"1",
						title:"facedawnssb",
						description:"qs",
						coverUrl:"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg",
						url:"",
					},
					{
						id:"2",
						title:"facedawnssb",
						description:"qs",
						coverUrl:"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg",
						url:"",
					}
				],
				
				userInfo:[
					{
						imgUrl:"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg",
						id:"123",
						username:"facedawn",
						description:"",
						url:"",
					}
				]
			}
		},
		onLoad(prop){
			this.searchCondition=prop.searchCondition;
		},
		methods: {
			clearCondition() {
				this.searchCondition = ''
			},
			search(){
				
			},
			pageChange(e){
				//重新加载applicationList
				this.applicationList.page=e.current;
				console.log(this.applicationList.page);
			},
			 tabtap(index){
			    this.tabIndex=index;
			},
			clickMessage(messageId){
				//到具体消息页面
			},
			clickUser(userId){
				//到对应用户界面
			},
			bindPickerChange: function(e) {		//改变的事件名
				//console.log('picker发送选择改变，携带值为', e.target.value)   用于输出改变索引值
				this.index = e.target.value			//将数组改变索引赋给定义的index变量
				this.jg=this.array[this.index]		//将array【改变索引】的值赋给定义的jg变量
				console.log("类型为：",this.jg)		//输出获取的籍贯值，例如：中国
			},
			
				
		}
	}
</script>

<style>

	.search_box {
		display: flex;
		background-color: #F3F3F3;
		margin-left: 5%;
		shape-margin: 5%;
		margin-top: 10px;
		margin-bottom: 10px;
		border-radius: 20rpx;
		width: 90%;
		height: 50rpx;
		top: 200rpx;
	}
	.search_box .search_icon {
		margin-left: 20rpx;
		margin-top: 10rpx;
		width: 30rpx;
		height: 30rpx;
	}
	
	.search_box .search_area {
		margin-left: 10rpx;
		margin-top: 5rpx;
		width: 80%;
		height: 40rpx;
		font-size: 30rpx;
	}
	
	.search_box .search_cancel {
		margin-left: 20rpx;
		margin-top: 13rpx;
		width: 30rpx;
		height: 30rpx;
	}
	.tabs {
	    flex: 1;
	    flex-direction: column;
	    overflow: hidden;
	    background-color: #ffffff;
	    /* #ifdef MP-ALIPAY || MP-BAIDU */
	    height: 100vh;
	    /* #endif */
	}
	
	.scroll-h {
	    width: 750upx;
	    height: 80upx;
	    flex-direction: row;
	    /* #ifndef APP-PLUS */
	    white-space: nowrap;
	    /* #endif */
	    /* flex-wrap: nowrap; */
	    /* border-color: #cccccc;
	    border-bottom-style: solid;
	    border-bottom-width: 1px; */
	}
	
	.line-h {
	    height: 1upx;
	    background-color: #cccccc;
	}
	
	.uni-tab-item {
	    /* #ifndef APP-PLUS */
	    display: inline-block;
	    /* #endif */
	    flex-wrap: nowrap;
	    padding-left: 34upx;
	    padding-right: 34upx;
	}
	
	.uni-tab-item-title {
	    color: #555;
	    font-size: 30upx;
	    height: 80upx;
	    line-height: 80upx;
	    flex-wrap: nowrap;
	    /* #ifndef APP-PLUS */
	    white-space: nowrap;
	    /* #endif */
	}
	
	.uni-tab-item-title-active {
	    color: #007AFF;
	}
	.image-1 {
			flex-shrink: 0;
			margin-right: 10px;
			width: 25%;
			height: 125px;
			border-radius: 6px;
			overflow: hidden;
			border: 1px #f5f5f5 solid;
	}
	.mid{
		left:50%; top:50%;
		
		transform:translate(-50%,-50%);
		
		-webkit-transform:translate(-50%,-50%);
		
		background-color:gray; color:white; position:absolute;
	}
	.Absolute-Center {
	
	  width: 50%;
	
	  height: 50%;
	
	  overflow: auto;
	
	  margin: auto;
	
	  position: absolute;
	
	  top: 0; left: 0; bottom: 0; right: 0;
	
	}
</style>
