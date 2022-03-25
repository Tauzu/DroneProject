# Drothy \~*Drone Simulator*~
作者氏名：井田　悠太

## デモ動画
https://user-images.githubusercontent.com/87505664/158532013-4edd8f36-20cc-439c-805c-b595b95ea691.mp4

## 概要
- ドローンを操縦して、ミッションクリアを目指すゲームです
- ミッションは全部で5つです
- Unityを使って制作しました
  - 自作のスクリプトは、`Assets/MyAsset/Scripts/`内にあります
  - シーンは、`Assets/MyAsset/Scenes/town.unity`を読み込んでください

## こだわったポイント
- 物理演算を積極的に使いました
  - ドローン
    - 移動は、`Rigidbody.AddForceAtPosition`メソッドのみで行っています
    - ドローンの羽根4か所に働く力を制御することで、ドローンを操ります
    - 姿勢制御のプログラムも組み込みました
    - 詳細は`Assets/MyAsset/Scripts/DronePhysics.cs`をご覧ください
  - 種々のオブジェクトに`RigidBody`を与え、衝突などの力学的な挙動が頻繁に起こるように工夫しました
  - 磁場オブジェクトをONにすれば、磁力で物体を引き寄せることもできます
    - あるミッションでは、この能力を活かして宅配をします
- 弾丸でいろんなものを壊すことができます
  - 弾丸を当てることでたいていのオブジェクトは壊すことができます
  - 敵も倒せます

## 操作方法
- \[Shift(L)]:上昇
- \[Ctrl(L)]:下降
- \[C]:ホバリング切替
- \[WASD]:移動
- \[Q]:加速
- \[Space]:弾丸発射
- \[E]:磁場発生
- \[X]:周囲破壊
  - これを使うと、磁力に引き寄せられた余計なモノを破壊することができます
  - 使用中、ドローンに無理な力がかかり、操縦不能に陥ります
  - 建物も破壊してしまうので、使う際は注意してください

- \[↑←↓→]:カメラ操作
- \[Shif(R)]:ズームイン
- \[Ctrl(R)]:ズームアウト
- \[Enter]:視点切替
- \[P]:カメラリバース切替

## 攻略方法
- Mission1
  - [Shit(L)]を長押しして上昇するだけです。
- Mission2
  - [C]を押すとホバリング状態に切り替わります。
  - 高さは[Shit(L)][Ctrl(L)]で調整してください。
  - ホバリング状態では、[WASD]で水平移動ができます。
  - 黄色いコインを10枚すべて通過するとクリアです。
- Mission3
  - [E]を押すと磁力状態に切り替わります。
  - 宅配物は磁力で引き寄せることができます。
  - 宅配物の届け先は、緑色の光が立ち上っている建物です。
  - 磁力で余計なモノを引き寄せてしまった場合は、[X]で破壊できます。
  - スコア6000点でクリアです。
- Mission4
  - [Enter]を押すと一人称視点に切り替わります。
  - [↑←↓→]でカメラを操作して敵に狙いを定め、[Space]で弾丸を発射してください。
  - 最初の敵を撃破後、敵発生装置が現れます。これを壊さない限り、無限に敵が発生するので、なるべく早く破壊してください。
  - 発生装置と敵全滅でクリアです。
- Mission5
  - ドラゴンには、普通の弾丸は通用しません。
  - スペシャルな宅配物が時々現れるので、それを届け先に届けてください。すると弾丸がパワーアップします。
  - パワーアップした弾丸で、ドラゴンを攻撃してください。緑色のバーがなくなれば撃破です。

ミッションは以上です

## おまけ
プレイ中に[G]キーを押すと、ドローンを増やせます。ハチャメチャを楽しむことができますが、操作がやりづらくなります。
